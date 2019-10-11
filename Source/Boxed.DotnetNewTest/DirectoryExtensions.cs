namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    internal static class DirectoryExtensions
    {
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            if (sourceDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(sourceDirectoryPath));
            }

            if (destinationDirectoryPath == null)
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

        public static void CheckCreate(string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            var destinationSubDirectory = new DirectoryInfo(directoryPath);
            if (!destinationSubDirectory.Exists)
            {
                destinationSubDirectory.Create();
            }
        }

        public static string GetTempDirectoryPath() =>
            Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        public static string GetCurrentDirectory() =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task<bool> TryDeleteDirectoryAsync(
           string directoryPath,
           int maxRetries = 10,
           int millisecondsDelay = 30)
        {
            if (directoryPath == null)
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
