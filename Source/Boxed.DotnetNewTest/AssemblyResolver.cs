#if NETCOREAPP3_0
namespace Boxed.DotnetNewTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.DependencyModel.Resolution;

    /// <summary>
    /// An assembly resolver.
    /// </summary>
    internal sealed class AssemblyResolver : IDisposable
    {
        private readonly ICompilationAssemblyResolver assemblyResolver;
        private readonly DependencyContext dependencyContext;
        private readonly AssemblyLoadContext loadContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        /// <param name="path">The path to load assemblies from.</param>
        public AssemblyResolver(string path)
        {
            this.Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            this.dependencyContext = DependencyContext.Load(this.Assembly);

            this.assemblyResolver = new CompositeCompilationAssemblyResolver(
                new ICompilationAssemblyResolver[]
                {
                    new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
                    new ReferenceAssemblyPathResolver(),
                    new PackageCompilationAssemblyResolver(),
                });

            this.loadContext = AssemblyLoadContext.GetLoadContext(this.Assembly);
            this.loadContext.Resolving += this.OnResolving;
        }

        /// <summary>
        /// Gets the assembly load context.
        /// </summary>
        public Assembly Assembly { get; }

        /// <inheritdoc/>
        public void Dispose() => this.loadContext.Resolving -= this.OnResolving;

        private Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            bool NamesMatch(RuntimeLibrary runtime) =>
                string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);

            var library = this.dependencyContext.RuntimeLibraries.FirstOrDefault(NamesMatch);
            if (library != null)
            {
                var wrapper = new CompilationLibrary(
                    library.Type,
                    library.Name,
                    library.Version,
                    library.Hash,
                    library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                    library.Dependencies,
                    library.Serviceable);

                var assemblies = new List<string>();
                this.assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies);
                if (assemblies.Count > 0)
                {
                    return this.loadContext.LoadFromAssemblyPath(assemblies[0]);
                }
            }

            return null;
        }
    }
}
#endif
