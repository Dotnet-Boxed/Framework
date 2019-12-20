namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Directory extension methods.
    /// </summary>
    internal static class DirectoryExtensions
    {
        /// <summary>
        /// Copies the specified source directory to the destination directory.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sourceDirectoryPath"/> or
        /// <paramref name="destinationDirectoryPath"/> are <c>null</c>.</exception>
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            if (sourceDirectoryPath is null)
            {
                throw new ArgumentNullException(nameof(sourceDirectoryPath));
            }

            if (destinationDirectoryPath is null)
            {
                throw new ArgumentNullException(nameof(destinationDirectoryPath));
            }

            sourceDirectoryPath = sourceDirectoryPath.TrimEnd('\\');

            foreach (var sourceSubDirectoryPath in Directory.GetDirectories(
                sourceDirectoryPath,
                "*",
                SearchOption.AllDirectories))
            {
                var destinationSubDirectoryPath = sourceSubDirectoryPath.Replace(
                    sourceDirectoryPath,
                    destinationDirectoryPath,
                    StringComparison.Ordinal);
                CheckCreate(destinationSubDirectoryPath);
            }

            foreach (var sourceFilePath in Directory.GetFiles(
                sourceDirectoryPath,
                "*.*",
                SearchOption.AllDirectories))
            {
                var destinationFilePath = sourceFilePath.Replace(
                    sourceDirectoryPath,
                    destinationDirectoryPath,
                    StringComparison.Ordinal);
                new FileInfo(sourceFilePath).CopyTo(destinationFilePath, true);
            }
        }

        /// <summary>
        /// Creates the specified directory if it doesn't exist.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <c>null</c>.</exception>
        public static void CheckCreate(string directoryPath)
        {
            if (directoryPath is null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            var destinationSubDirectory = new DirectoryInfo(directoryPath);
            if (!destinationSubDirectory.Exists)
            {
                destinationSubDirectory.Create();
            }
        }

        /// <summary>
        /// Gets the temporary directory path.
        /// </summary>
        /// <returns>The temporary directory path.</returns>
        public static string GetTempDirectoryPath() =>
            Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Replace("-", string.Empty, StringComparison.Ordinal));

        /// <summary>
        /// Gets a short temporary directory path to work around the MSBuild 256 character file path limit.
        /// </summary>
        /// <returns>A short temporary directory path.</returns>
        public static string GetShortTempDirectoryPath() =>
            Path.Combine(
                DriveInfo.GetDrives().First().RootDirectory.FullName,
                "Temp",
                Guid.NewGuid().ToString().Replace("-", string.Empty, StringComparison.Ordinal));

        /// <summary>
        /// Gets the current directory for the currently executing assembly.
        /// </summary>
        /// <returns>The directory for the currently executing assembly.</returns>
        public static string GetCurrentDirectory() =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Tries to delete the specified directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <param name="millisecondsDelay">The milliseconds delay between retries.</param>
        /// <returns><c>true</c> if the directory was deleted, otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxRetries"/> or
        /// <paramref name="millisecondsDelay"/> is less than one.</exception>
        public static async Task<bool> TryDeleteDirectoryAsync(
           string directoryPath,
           int maxRetries = 10,
           int millisecondsDelay = 30)
        {
            if (directoryPath is null)
            {
                throw new ArgumentNullException(directoryPath);
            }

            if (maxRetries < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            if (millisecondsDelay < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
            }

            for (var i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true);
                    }

                    return true;
                }
                catch (IOException)
                {
                    await Task.Delay(millisecondsDelay).ConfigureAwait(false);
                }
                catch (UnauthorizedAccessException)
                {
                    await Task.Delay(millisecondsDelay).ConfigureAwait(false);
                }
            }

            return false;
        }
    }
}
