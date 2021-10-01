namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Runs 'dotnet new' commands.
    /// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public static class DotnetNew
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <typeparam name="T">A type from the assembly used to find the directory path of the project to install.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task InstallAsync<T>(string fileName) =>
            InstallAsync(typeof(T).GetTypeInfo().Assembly, fileName);

        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <param name="assembly">The assembly used to find the directory path of the project to install.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <para><paramref name="assembly"/> is null. </para>
        ///     <para>- or - </para>
        ///     <para><paramref name="fileName"/> is null. </para>
        /// </exception>
        /// <exception cref="FileNotFoundException">A file with the specified file name was not found.</exception>
        public static Task InstallAsync(Assembly assembly, string fileName)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException(nameof(fileName), $"{nameof(fileName)} must not be empty.");
            }

            var projectFilePath = GetProjectFilePath(assembly, fileName);

            return InstallAsync(projectFilePath);
        }

        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The provided <paramref name="source"/> was null.</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="source"/> was empty.</exception>
        public static async Task InstallAsync(string source, TimeSpan? timeout = null, bool showShellWindow = false)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length == 0)
            {
                throw new ArgumentException(nameof(source), $"{nameof(source)} must not be empty.");
            }

            await RunDotnetCommandAsync($"new --install \"{source}\"", timeout, showShellWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Reinitialises the dotnet new command.
        /// </summary>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ReinitialiseAsync(TimeSpan? timeout = null, bool showShellWindow = false)
        {
            await RunDotnetCommandAsync($"new --debug:reinit", timeout, showShellWindow).ConfigureAwait(false);
        }

        /// <summary>
        /// Uninstalls a template from the specified source.
        /// </summary>
        /// <typeparam name="T">A type from the assembly used to find the directory path of the project to install.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task UninstallAsync<T>(string fileName) =>
            UninstallAsync(typeof(T).GetTypeInfo().Assembly, fileName);

        /// <summary>
        /// Uninstalls a template from the specified source.
        /// </summary>
        /// <param name="assembly">The assembly used to find the directory path of the project to install.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <para><paramref name="assembly"/> is null. </para>
        ///     <para>- or - </para>
        ///     <para><paramref name="fileName"/> is null. </para>
        /// </exception>
        /// <exception cref="FileNotFoundException">A file with the specified file name was not found.</exception>
        public static Task UninstallAsync(Assembly assembly, string fileName)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException(nameof(fileName), $"{nameof(fileName)} must not be empty.");
            }

            var projectFilePath = GetProjectFilePath(assembly, fileName);

            return UninstallAsync(projectFilePath);
        }

        /// <summary>
        /// Uninstalls a template from the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The provided <paramref name="source"/> was null.</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="source"/> was empty.</exception>
        public static async Task UninstallAsync(string source, TimeSpan? timeout = null, bool showShellWindow = false)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length == 0)
            {
                throw new ArgumentException(nameof(source), $"{nameof(source)} must not be empty.");
            }

            await RunDotnetCommandAsync($"new --uninstall \"{source}\"", timeout, showShellWindow).ConfigureAwait(false);
        }

        private static string GetProjectFilePath(Assembly assembly, string fileName)
        {
            var projectFilePath = Path.GetDirectoryName(GetFilePath(assembly, fileName));
            if (projectFilePath is null)
            {
                throw new FileNotFoundException($"{fileName} not found.");
            }

            return projectFilePath;
        }

        private static string? GetFilePath(Assembly assembly, string projectName)
        {
            string? projectFilePath = null;

            for (var directory = new DirectoryInfo(assembly.Location); directory.Parent is not null; directory = directory.Parent)
            {
                projectFilePath = directory
                    .Parent
                    .GetFiles(projectName, SearchOption.AllDirectories)
                    .FirstOrDefault(x => !IsInObjDirectory(x.Directory))
                    ?.FullName;
                if (projectFilePath is not null)
                {
                    break;
                }
            }

            return projectFilePath;
        }

        private static bool IsInObjDirectory(DirectoryInfo? directoryInfo)
        {
            if (directoryInfo is null)
            {
                return false;
            }
            else if (string.Equals(directoryInfo.Name, "obj", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return IsInObjDirectory(directoryInfo.Parent);
        }

        private static async Task<(ProcessResult ProcessResult, string Message)> RunDotnetCommandAsync(string arguments, TimeSpan? timeout, bool showShellWindow)
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout);
            return await ProcessExtensions
                    .StartAsync(
                        DirectoryExtensions.GetCurrentDirectory(),
                        "dotnet",
                        arguments,
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
        }
    }
}
