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
var versionSuffix = string.IsNullOrEmpty(preReleaseSuffix) ? null : preReleaseSuffix + "-" + buildNumber.ToString("D4");

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
        DeleteDirectories(GetDirectories("**/bin"), new DeleteDirectorySettings() { Force = true, Recursive = true });
        DeleteDirectories(GetDirectories("**/obj"), new DeleteDirectorySettings() { Force = true, Recursive = true });
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
                Configuration = configuration,
                NoRestore = true,
                VersionSuffix = versionSuffix
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
            var outputFilePath = MakeAbsolute(artifactsDirectory.Path)
                .CombineWithFilePath(project.GetFilenameWithoutExtension());
            var arguments = new ProcessArgumentBuilder()
                .AppendSwitch("-configuration", configuration)
                .AppendSwitchQuoted("-xml", outputFilePath.AppendExtension(".xml").ToString())
                .AppendSwitchQuoted("-html", outputFilePath.AppendExtension(".html").ToString());

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
                    arguments.AppendSwitch("-framework", frameworks.First());
                }
            }

            DotNetCoreTool(project, "xunit", arguments);
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        DotNetCorePack(
            ".",
            new DotNetCorePackSettings()
            {
                Configuration = configuration,
                OutputDirectory = artifactsDirectory,
                VersionSuffix = versionSuffix
            });
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);