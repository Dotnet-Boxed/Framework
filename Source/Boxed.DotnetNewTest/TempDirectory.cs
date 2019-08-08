namespace Boxed.DotnetNewTest
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Creates a temporary directory at the specified directory path and deletes it when <see cref="Dispose"/> is called.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class TempDirectory : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="directoryPath">The temporary directory path.</param>
        public TempDirectory(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
            DirectoryExtensions.CheckCreate(this.DirectoryPath);
        }

        /// <summary>
        /// Gets the temporary directory path.
        /// </summary>
        public string DirectoryPath { get; }

        /// <summary>
        /// Creates a new <see cref="TempDirectory"/>.
        /// </summary>
        /// <returns>The temporary directory.</returns>
        public static TempDirectory NewTempDirectory() => new TempDirectory(DirectoryExtensions.GetTempDirectoryPath());

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            var task = DirectoryExtensions.TryDeleteDirectory(this.DirectoryPath);
            task.Wait();
            if (!task.Result)
            {
                Debug.WriteLine($"Failed to delete directory {this.DirectoryPath}");
            }
        }
    }
}
