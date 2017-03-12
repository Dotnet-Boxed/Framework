using System.Text.RegularExpressions;
using System.Xml.Linq;

var target = Argument("Target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") :
    "Release";
var preReleaseSuffix =
    HasArgument("PreReleaseSuffix") ? Argument<string>("PreReleaseSuffix") :
    (AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag) ? null :
    EnvironmentVariable("PreReleaseSuffix") != null ? EnvironmentVariable("PreReleaseSuffix") :
    "beta";
var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber :
    EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) :
    0;

var artifactsDirectory = Directory("./Artifacts");

IList<string> GetCoreFrameworks(string csprojFilePath)
{
    return XDocument
        .Load(csprojFilePath)
        .Descendants("TargetFrameworks")
        .First()
        .Value
        .Split(';')
        .Where(x => Regex.IsMatch(x, @"net[^\d]"))
        .ToList();
}

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
        DeleteDirectories(GetDirectories("**/bin"), true);
        DeleteDirectories(GetDirectories("**/obj"), true);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

 Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        foreach(var project in GetFiles("./**/*.csproj"))
        {
            Information(project.ToString());
            var settings = new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            };

            if (!IsRunningOnWindows())
            {
                var frameworks = GetCoreFrameworks(project.ToString());
                if (frameworks.Count == 0)
                {
                    Information("Skipping .NET Framework only project " + project.ToString());
                    continue;
                }
                else
                {
                    Information("Skipping .NET Framework, building " + frameworks.First());
                    settings.Framework = frameworks.First();
                }
            }

            DotNetCoreBuild(
                project.GetDirectory().FullPath,
                settings);
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach(var project in GetFiles("./Tests/**/*.csproj"))
        {
            var settings = new DotNetCoreTestSettings()
            {
                //ArgumentCustomization = args => args
                //    .Append("-xml")
                //    .Append(artifactsDirectory.Path.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml"),
                Configuration = configuration,
                NoBuild = true
            };

            if (!IsRunningOnWindows())
            {
                var frameworks = GetCoreFrameworks(project.ToString());
                if (frameworks.Count == 0)
                {
                    Information("Skipping .NET Framework only project " + project.ToString());
                    continue;
                }
                else
                {
                    Information("Skipping .NET Framework, building " + frameworks.First());
                    settings.Framework = frameworks.First();
                }
            }

            DotNetCoreTest(
                project.ToString(),
                settings);
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        string versionSuffix = null;
        if (!string.IsNullOrEmpty(preReleaseSuffix))
        {
            versionSuffix = preReleaseSuffix + "-" + buildNumber.ToString("D4");
        }

        foreach (var project in GetFiles("./Source/**/*.csproj"))
        {
            DotNetCorePack(
                project.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = artifactsDirectory,
                    VersionSuffix = versionSuffix
                });
        }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);