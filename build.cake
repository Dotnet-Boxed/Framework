var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var artifactsDirectory = Directory("./Artifacts");

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
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
        var projects = GetFiles("./**/*.xproj");
        foreach(var project in projects)
        {
            DotNetCoreBuild(
                project.GetDirectory().FullPath,
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration
                });
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./Tests/**/*.xproj");
        foreach(var project in projects)
        {
            if(IsRunningOnWindows())
            {
                DotNetCoreTest(
                    project.GetDirectory().FullPath,
                    new DotNetCoreTestSettings()
                    {
                        ArgumentCustomization = args => args
                            .Append("-xml")
                            .Append(artifactsDirectory.Path.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml"),
                        Configuration = configuration,
                        NoBuild = true
                    });
            }
            else
            {
                var name = project.GetFilenameWithoutExtension();
                var dirPath = project.GetDirectory().FullPath;
                var xunit = GetFiles(dirPath + "/bin/" + configuration + "/net451/*/dotnet-test-xunit.exe").First().FullPath;
                Information("dotnet-test-xunit.exe File Path: " + xunit);
                var testfile = GetFiles(dirPath + "/bin/" + configuration + "/net451/*/" + name + ".dll").First().FullPath;
                Information("Assembly File Path: " + xunit);

                using (var process = StartAndReturnProcess("mono", new ProcessSettings{ Arguments = xunit + " " + testfile }))
                {
                    process.WaitForExit();
                    if (process.GetExitCode() != 0)
                    {
                        throw new Exception("Mono tests failed!");
                    }
                }
            }
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        int buildNumber;
        if (AppVeyor.IsRunningOnAppVeyor)
        {
            buildNumber = AppVeyor.Environment.Build.Number;
        }
        else if (TravisCI.IsRunningOnTravisCI)
        {
            buildNumber = TravisCI.Environment.Build.BuildNumber;
        }
        else
        {
            buildNumber = 1;
        }
        var revision = buildNumber.ToString("D4");

        foreach (var project in GetFiles("./Source/**/*.xproj"))
        {
            DotNetCorePack(
                project.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = artifactsDirectory,
                    VersionSuffix = revision
                });
        }
    });

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Pack")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        foreach(var file in GetFiles(artifactsDirectory.Path + "/*"))
        {
            AppVeyor.UploadArtifact(file);
        }
    });

Task("Default")
    .IsDependentOn("Upload-AppVeyor-Artifacts");

RunTarget(target);