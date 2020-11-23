namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// <see cref="Project"/> extension methods.
    /// </summary>
    public static class ProjectExtensions
    {
        private static readonly string[] DefaultUrls = new string[] { "http://localhost", "https://localhost" };

        /// <summary>
        /// Runs 'dotnet restore' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetRestoreAsync(
            this Project project,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        "restore",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet build' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetBuildAsync(
            this Project project,
            bool? noRestore = true,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var noRestoreArgument = noRestore is null ? null : "--no-restore";
            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        $"build {noRestoreArgument}",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet test' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetTestAsync(
            this Project project,
            bool? noRestore = true,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var noRestoreArgument = noRestore is null ? null : "--no-restore";
            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        $"test {noRestoreArgument}",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet tool restore' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetToolRestoreAsync(
            this Project project,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        $"tool restore",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet cake' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="target">The target to run.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetCakeAsync(
            this Project project,
            string target = null,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var targetArgument = target is null ? null : $"--target={target}";
            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        $"cake {targetArgument}",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet publish' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="framework">The framework.</param>
        /// <param name="runtime">The runtime.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetPublishAsync(
            this Project project,
            string framework = null,
            string runtime = null,
            bool? noRestore = true,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var frameworkArgument = framework is null ? null : $"--framework {framework}";
            var runtimeArgument = runtime is null ? null : $"--self-contained --runtime {runtime}";
            var noRestoreArgument = noRestore is null ? null : "--no-restore";
            DirectoryExtensions.CheckCreate(project.PublishDirectoryPath);
            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await AssertStartAsync(
                        project.DirectoryPath,
                        "dotnet",
                        $"publish {noRestoreArgument} {frameworkArgument} {runtimeArgument} --output {project.PublishDirectoryPath}",
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet run' on the specified project while only exposing a HTTP endpoint.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectRelativeDirectoryPath">The project relative directory path.</param>
        /// <param name="readinessCheck">The readiness check to perform to check that the app has started.</param>
        /// <param name="action">The action to perform while the project is running.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="validateCertificate">Validate the project certificate.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetRunAsync(
            this Project project,
            string projectRelativeDirectoryPath,
            Func<HttpClient, Task<bool>> readinessCheck,
            Func<HttpClient, Task> action,
            bool? noRestore = true,
            Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> validateCertificate = null,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (projectRelativeDirectoryPath is null)
            {
                throw new ArgumentNullException(nameof(projectRelativeDirectoryPath));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var httpPort = PortHelper.GetFreeTcpPort();
            var httpUrl = new Uri($"http://localhost:{httpPort}");

            var httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = validateCertificate ?? DefaultValidateCertificate,
            };
            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = httpUrl };

            var projectFilePath = Path.Combine(project.DirectoryPath, projectRelativeDirectoryPath);
            var dotnetRun = await DotnetRunInternalAsync(
                    (http, https) => readinessCheck(http),
                    httpClient,
                    httpsClient: null,
                    projectFilePath,
                    noRestore,
                    timeout,
                    showShellWindow,
                    httpUrl)
                .ConfigureAwait(false);

            try
            {
                await action(httpClient).ConfigureAwait(false);
            }
            finally
            {
                httpClient.Dispose();
                httpClientHandler.Dispose();
                await dotnetRun.DisposeAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs 'dotnet run' on the specified project while only exposing a HTTP and HTTPS endpoint.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectRelativeDirectoryPath">The project relative directory path.</param>
        /// <param name="readinessCheck">The readiness check to perform to check that the app has started.</param>
        /// <param name="action">The action to perform while the project is running.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="validateCertificate">Validate the project certificate.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetRunAsync(
            this Project project,
            string projectRelativeDirectoryPath,
            Func<HttpClient, HttpClient, Task<bool>> readinessCheck,
            Func<HttpClient, HttpClient, Task> action,
            bool? noRestore = true,
            Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> validateCertificate = null,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (projectRelativeDirectoryPath is null)
            {
                throw new ArgumentNullException(nameof(projectRelativeDirectoryPath));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var httpPort = PortHelper.GetFreeTcpPort();
            var httpsPort = PortHelper.GetFreeTcpPort();
            var httpUrl = new Uri($"http://localhost:{httpPort}");
            var httpsUrl = new Uri($"https://localhost:{httpsPort}");

            var httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = validateCertificate ?? DefaultValidateCertificate,
            };
            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = httpUrl };
            var httpsClient = new HttpClient(httpClientHandler) { BaseAddress = httpsUrl };

            var projectFilePath = Path.Combine(project.DirectoryPath, projectRelativeDirectoryPath);
            var dotnetRun = await DotnetRunInternalAsync(
                    readinessCheck,
                    httpClient,
                    httpsClient,
                    projectFilePath,
                    noRestore,
                    timeout,
                    showShellWindow,
                    httpUrl,
                    httpsUrl)
                .ConfigureAwait(false);

            try
            {
                await action(httpClient, httpsClient).ConfigureAwait(false);
            }
            finally
            {
                httpClient.Dispose();
                httpsClient.Dispose();
                httpClientHandler.Dispose();
                await dotnetRun.DisposeAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs the project in-memory.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="action">The action to perform while the project is running.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="startupTypeName">Name of the startup type.</param>
        /// <returns>A task representing the operation.</returns>
        /// <remarks>This doesn't work yet, needs API's from .NET Core 3.0.</remarks>
        internal static async Task DotnetRunInMemoryAsync(
            this Project project,
            Func<TestServer, Task> action,
            string environmentName = "Development",
            string startupTypeName = "Startup")
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var projectName = Path.GetFileName(project.DirectoryPath);
            var directoryPath = project.PublishDirectoryPath;
            var assemblyFilePath = Path.Combine(directoryPath, $"{projectName}.dll");

            if (string.IsNullOrEmpty(assemblyFilePath))
            {
                throw new FileNotFoundException(Resources.ProjectAssemblyFileNotFound, assemblyFilePath);
            }
            else
            {
                using (var assemblyResolver = new AssemblyResolver(assemblyFilePath))
                {
                    var assembly = assemblyResolver.Assembly;
                    var startupType = assembly
                        .DefinedTypes
                        .FirstOrDefault(x => string.Equals(x.Name, startupTypeName, StringComparison.Ordinal));
                    if (startupType is null)
                    {
                        throw new Exception($"Startup type '{startupTypeName}' not found.");
                    }

                    var webHostBuilder = new WebHostBuilder()
                        .UseEnvironment(environmentName)
                        .UseStartup(startupType)
                        .UseUrls(DefaultUrls);
                    using (var testServer = new TestServer(webHostBuilder))
                    {
                        await action(testServer).ConfigureAwait(false);
                    }

                    // TODO: Unload startupType when supported: https://github.com/dotnet/corefx/issues/14724
                }
            }
        }

        private static async Task<IAsyncDisposable> DotnetRunInternalAsync(
            Func<HttpClient, HttpClient, Task<bool>> readinessCheck,
            HttpClient httpClient,
            HttpClient httpsClient,
            string directoryPath,
            bool? noRestore,
            TimeSpan? timeout,
            bool showShellWindow,
            params Uri[] urls)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope. Object disposed below.
            var cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore CA2000 // Dispose objects before losing scope. Object disposed below.
            var noRestoreArgument = noRestore is null ? null : "--no-restore";
            var urlsParameter = string.Join(";", urls.Select(x => x.ToString()));
            var task = AssertStartAsync(
                directoryPath,
                "dotnet",
                $"run {noRestoreArgument} --urls {urlsParameter}",
                showShellWindow,
                cancellationTokenSource.Token);

            try
            {
                await WaitForStartAsync(
                        readinessCheck,
                        httpClient,
                        httpsClient,
                        timeout ?? ConfigurationService.DefaultTimeout)
                    .ConfigureAwait(false);
            }
            catch
            {
                await Dispose(cancellationTokenSource, task, showShellWindow).ConfigureAwait(false);
                throw;
            }

            return new AsyncDisposableAction(() => Dispose(cancellationTokenSource, task, showShellWindow));

            async static ValueTask Dispose(CancellationTokenSource cancellationTokenSource, Task task, bool showShellWindow)
            {
                if (showShellWindow)
                {
                    await Task.Delay(3000).ConfigureAwait(false);
                }

                cancellationTokenSource.Cancel();

                try
                {
                    await task.ConfigureAwait(false);
                }
                catch (Exception exception)
                when (exception.GetBaseException() is TaskCanceledException)
                {
                }
                finally
                {
                    cancellationTokenSource.Dispose();
                }
            }
        }

        private static async Task WaitForStartAsync(
            Func<HttpClient, HttpClient, Task<bool>> readinessCheck,
            HttpClient httpClient,
            HttpClient httpsClient,
            TimeSpan timeout)
        {
            const int intervalMilliseconds = 100;

            TestLogger.WriteLine(Resources.WaitingForAppToStartAndPassReadinessCheck);

            using (var cancellationTokenSource = new CancellationTokenSource(timeout))
            {
                while (true)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var message = $"Timed out after waiting {timeout} for app to start using dotnet run.";
                        TestLogger.WriteLine(message);
                        throw new TimeoutException(message);
                    }

                    try
                    {
                        var isSuccess = await readinessCheck(httpClient, httpsClient).ConfigureAwait(false);
                        if (isSuccess)
                        {
                            TestLogger.WriteLine(Resources.AppHasStartedAndReadinessCheckHasSucceeded);
                            return;
                        }
                        else
                        {
                            TestLogger.WriteLine(Resources.WaitingForAppToStartReadinessCheckFailed);
                        }
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        TestLogger.WriteLine(Resources.WaitingForAppToStartReadinessCheckThrewException);
                    }

                    await Task.Delay(intervalMilliseconds).ConfigureAwait(false);
                }
            }
        }

        private static bool DefaultValidateCertificate(
            HttpRequestMessage request,
            X509Certificate2 certificate,
            X509Chain chain,
            SslPolicyErrors errors) => true;

        private static async Task AssertStartAsync(
            string workingDirectory,
            string fileName,
            string arguments,
            bool showShellWindow,
            CancellationToken cancellationToken)
        {
            var (processResult, message) = await ProcessExtensions.StartAsync(
                workingDirectory,
                fileName,
                arguments,
                showShellWindow,
                cancellationToken).ConfigureAwait(false);
            if (processResult != ProcessResult.Succeeded)
            {
                throw new Exception(message);
            }
        }
    }
}
