namespace Boxed.DotnetNewTest
{
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
        public Project(
            string name,
            string directoryPath,
            string publishDirectoryPath)
        {
            this.Name = name;
            this.DirectoryPath = directoryPath;
            this.PublishDirectoryPath = publishDirectoryPath;
        }

        /// <summary>
        /// Gets the directory path to the project.
        /// </summary>
        public string DirectoryPath { get; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the publish directory path to the project.
        /// </summary>
        public string PublishDirectoryPath { get; }
    }
}
