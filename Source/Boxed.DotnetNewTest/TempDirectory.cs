namespace Boxed.DotnetNewTest
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Creates a temporary directory at the specified directory path and deletes it when <see cref="IDisposable.Dispose"/> is called.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class TempDirectory : IAsyncDisposable
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
        public static TempDirectory NewTempDirectory() =>
            new TempDirectory(DirectoryExtensions.GetTempDirectoryPath());

        /// <summary>
        /// Creates a new <see cref="TempDirectory"/> using a shorter directory path to work around the MSBuild 256
        /// character file path limit.
        /// </summary>
        /// <returns>The temporary directory.</returns>
        public static TempDirectory NewShortTempDirectory() =>
            new TempDirectory(DirectoryExtensions.GetShortTempDirectoryPath());

        /// <summary>
        /// Disposes the managed resources implementing <see cref="IDisposable" />.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            var isDeleted = await DirectoryExtensions
                .TryDeleteDirectoryAsync(this.DirectoryPath)
                .ConfigureAwait(false);
            if (!isDeleted)
            {
                Debug.WriteLine($"Failed to delete directory {this.DirectoryPath}");
            }
        }
    }
}
