namespace Boxed.DotnetNewTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Runs 'dotnet new' commands.
    /// </summary>
    public static class DotnetNew
    {
        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <typeparam name="T">A type from the assembly used to find the directory path of the project to install.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task Install<T>(string fileName) =>
            Install(typeof(T).GetTypeInfo().Assembly, fileName);

        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <param name="assembly">The assembly used to find the directory path of the project to install.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="FileNotFoundException">A file with the specified file name was not found.</exception>
        public static Task Install(Assembly assembly, string fileName)
        {
            var projectFilePath = Path.GetDirectoryName(GetFilePath(assembly, fileName));
            if (projectFilePath == null)
            {
                throw new FileNotFoundException($"{fileName} not found.");
            }

            return Install(projectFilePath);
        }

        /// <summary>
        /// Installs a template from the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task Install(string source, TimeSpan? timeout = null) =>
            ProcessExtensions.StartAsync(
                DirectoryExtensions.GetCurrentDirectory(),
                "dotnet",
                $"new --install \"{source}\"",
                CancellationTokenFactory.GetCancellationToken(timeout));

        private static string GetFilePath(Assembly assembly, string projectName)
        {
            string projectFilePath = null;

            var dllPath = new Uri(assembly.CodeBase).AbsolutePath;

            for (var directory = new DirectoryInfo(dllPath); directory.Parent != null; directory = directory.Parent)
            {
                projectFilePath = directory
                    .Parent
                    .GetFiles(projectName, SearchOption.AllDirectories)
                    .Where(x => !IsInObjDirectory(x.Directory))
                    .FirstOrDefault()
                    ?.FullName;
                if (projectFilePath != null)
                {
                    break;
                }
            }

            return projectFilePath;
        }

        private static bool IsInObjDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                return false;
            }
            else if (string.Equals(directoryInfo.Name, "obj", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return IsInObjDirectory(directoryInfo.Parent);
        }
    }
}
