namespace Boxed.DotnetNewTest
{
    using System;

    /// <summary>
    /// A project created from a project template.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <param name="directoryPath">The directory path to the project.</param>
        /// <param name="publishDirectoryPath">The publish directory path.</param>
        /// <param name="httpsPort">The HTTPS port.</param>
        /// <param name="httpPort">The HTTP port.</param>
        public Project(
            string? name,
            string directoryPath,
            string publishDirectoryPath,
            int httpsPort,
            int httpPort)
        {
            this.Name = name;
            this.DirectoryPath = directoryPath;
            this.PublishDirectoryPath = publishDirectoryPath;
            this.HttpsPort = httpsPort;
            this.HttpPort = httpPort;
        }

        /// <summary>
        /// Gets the directory path to the project.
        /// </summary>
        public string DirectoryPath { get; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the publish directory path to the project.
        /// </summary>
        public string PublishDirectoryPath { get; }

        /// <summary>
        /// Gets the HTTP port.
        /// </summary>
        public int HttpPort { get; }

        /// <summary>
        /// Gets the HTTPS port.
        /// </summary>
        public int HttpsPort { get; }

        /// <summary>
        /// Gets the HTTP URL.
        /// </summary>
        public Uri HttpUrl => new($"http://localhost:{this.HttpPort}");

        /// <summary>
        /// Gets the HTTPS URL.
        /// </summary>
        public Uri HttpsUrl => new($"https://localhost:{this.HttpsPort}");
    }
}
