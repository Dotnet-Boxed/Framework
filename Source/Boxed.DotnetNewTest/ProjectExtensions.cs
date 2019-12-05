namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
#if NETCOREAPP3_0
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Hosting;
#endif

    /// <summary>
    /// <see cref="Project"/> extension methods.
    /// </summary>
    public static class ProjectExtensions
    {
#if NETCOREAPP3_0
        private static readonly string[] DefaultUrls = new string[] { "http://localhost", "https://localhost" };
#endif

        /// <summary>
        /// Runs 'dotnet restore' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task DotnetRestoreAsync(
            this Project project,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            return AssertStartAsync(
                project.DirectoryPath,
                "dotnet",
                "restore",
                showShellWindow,
                CancellationTokenFactory.GetCancellationToken(timeout));
        }

        /// <summary>
        /// Runs 'dotnet build' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task DotnetBuildAsync(
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
            return AssertStartAsync(
                project.DirectoryPath,
                "dotnet",
                $"build {noRestoreArgument}",
                showShellWindow,
                CancellationTokenFactory.GetCancellationToken(timeout));
        }

        /// <summary>
        /// Runs 'dotnet publish' on the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="framework">The framework.</param>
        /// <param name="runtime">The runtime.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task DotnetPublishAsync(
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
            return AssertStartAsync(
                project.DirectoryPath,
                "dotnet",
                $"publish {noRestoreArgument} {frameworkArgument} {runtimeArgument} --output {project.PublishDirectoryPath}",
                showShellWindow,
                CancellationTokenFactory.GetCancellationToken(timeout));
        }

        /// <summary>
        /// Runs 'dotnet run' on the specified project while only exposing a HTTP endpoint.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectRelativeDirectoryPath">The project relative directory path.</param>
        /// <param name="action">The action to perform while the project is running.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="validateCertificate">Validate the project certificate.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetRunAsync(
            this Project project,
            string projectRelativeDirectoryPath,
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
            var httpUrl = $"http://localhost:{httpPort}";

            var projectFilePath = Path.Combine(project.DirectoryPath, projectRelativeDirectoryPath);
            var dotnetRun = await DotnetRunInternalAsync(projectFilePath, noRestore, timeout, showShellWindow, httpUrl)
                .ConfigureAwait(false);

            var httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = validateCertificate ?? DefaultValidateCertificate,
            };
            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(httpUrl) };

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
        /// <param name="action">The action to perform while the project is running.</param>
        /// <param name="noRestore">Whether to restore the project.</param>
        /// <param name="validateCertificate">Validate the project certificate.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A task representing the operation.</returns>
        public static async Task DotnetRunAsync(
            this Project project,
            string projectRelativeDirectoryPath,
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
            var httpUrl = $"http://localhost:{httpPort}";
            var httpsUrl = $"https://localhost:{httpsPort}";

            var projectFilePath = Path.Combine(project.DirectoryPath, projectRelativeDirectoryPath);
            var dotnetRun = await DotnetRunInternalAsync(projectFilePath, noRestore, timeout, showShellWindow, httpUrl, httpsUrl)
                .ConfigureAwait(false);

            var httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = validateCertificate ?? DefaultValidateCertificate,
            };
            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(httpUrl) };
            var httpsClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(httpsUrl) };

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

#if NETCOREAPP3_0
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
                throw new FileNotFoundException($"Project assembly not found.", assemblyFilePath);
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
#endif

        private static async Task<IAsyncDisposable> DotnetRunInternalAsync(
            string directoryPath,
            bool? noRestore,
            TimeSpan? timeout,
            bool showShellWindow,
            params string[] urls)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope. Object disposed below.
            var cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore CA2000 // Dispose objects before losing scope. Object disposed below.
            var noRestoreArgument = noRestore is null ? null : "--no-restore";
            var urlsParameter = string.Join(";", urls);
            var task = AssertStartAsync(
                directoryPath,
                "dotnet",
                $"run {noRestoreArgument} --urls {urlsParameter}",
                showShellWindow,
                cancellationTokenSource.Token);

            try
            {
                await WaitForStartAsync(urls.First(), timeout ?? TimeSpan.FromMinutes(1)).ConfigureAwait(false);
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

        private static async Task WaitForStartAsync(string url, TimeSpan timeout)
        {
            const int intervalMilliseconds = 100;

            using (var httpClientHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    ServerCertificateCustomValidationCallback = DefaultValidateCertificate,
                })
            {
                using (var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(url) })
                {
                    for (var i = 0; i < (timeout.TotalMilliseconds / intervalMilliseconds); ++i)
                    {
                        try
                        {
                            await httpClient.GetAsync(new Uri("/", UriKind.Relative)).ConfigureAwait(false);
                            return;
                        }
                        catch (HttpRequestException exception)
                        when (IsApiDownException(exception))
                        {
                            await Task.Delay(intervalMilliseconds).ConfigureAwait(false);
                        }
                    }

                    throw new TimeoutException(
                        $"Timed out after waiting {timeout} for application to start using dotnet run.");
                }
            }
        }

        private static bool IsApiDownException(Exception exception)
        {
            var result = false;
            var baseException = exception.GetBaseException();
            if (baseException is SocketException socketException)
            {
                result =
                    string.Equals(
                        socketException.Message,
                        "No connection could be made because the target machine actively refused it",
                        StringComparison.Ordinal)
                    ||
                    string.Equals(
                        socketException.Message,
                        "Connection refused",
                        StringComparison.Ordinal);
            }

            return result;
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
