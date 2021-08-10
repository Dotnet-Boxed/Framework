namespace Boxed.DotnetNewTest
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="TempDirectory"/> extension methods.
    /// </summary>
    public static class TempDirectoryExtensions
    {
        /// <summary>
        /// Runs 'dotnet new' with the specified arguments.
        /// </summary>
        /// <param name="tempDirectory">The temporary directory.</param>
        /// <param name="templateName">Name of the 'dotnet new' template to create.</param>
        /// <param name="name">The name of the project to create from the template.</param>
        /// <param name="arguments">The custom arguments to pass to the template.</param>
        /// <param name="timeout">The timeout. Defaults to one minute.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show the shell window instead of logging to output.</param>
        /// <returns>A project created from a project template.</returns>
        public static async Task<Project> DotnetNewAsync(
            this TempDirectory tempDirectory,
            string templateName,
            string? name = null,
            IDictionary<string, string>? arguments = null,
            TimeSpan? timeout = null,
            bool showShellWindow = false)
        {
            if (tempDirectory is null)
            {
                throw new ArgumentNullException(nameof(tempDirectory));
            }

            var stringBuilder = new StringBuilder($"new {templateName}");

            if (name != null)
            {
                stringBuilder.Append($" --name \"{name}\"");
            }

            var httpsPort = PortHelper.GetFreeTcpPort();
            var httpPort = PortHelper.GetFreeTcpPort();

            if (arguments is not null)
            {
                foreach (var argument in arguments)
                {
                    stringBuilder.Append($" --{argument.Key} \"{Replace(argument.Value, httpsPort, httpPort)}\"");
                }
            }

            using (var cancellationTokenSource = new CancellationTokenSource(timeout ?? ConfigurationService.DefaultTimeout))
            {
                await ProcessExtensions
                    .StartAsync(
                        tempDirectory.DirectoryPath,
                        "dotnet",
                        stringBuilder.ToString(),
                        showShellWindow,
                        cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }

            var projectDirectoryPath = name == null ? tempDirectory.DirectoryPath : Path.Combine(tempDirectory.DirectoryPath, name);
            var publishDirectoryPath = Path.Combine(projectDirectoryPath, "Publish");
            return new Project(name, projectDirectoryPath, publishDirectoryPath, httpsPort, httpPort);
        }

        private static string Replace(string value, int httpsPort, int httpPort) =>
            value
                .Replace("{HTTPS_PORT}", httpsPort.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal)
                .Replace("{HTTP_PORT}", httpPort.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
    }
}
