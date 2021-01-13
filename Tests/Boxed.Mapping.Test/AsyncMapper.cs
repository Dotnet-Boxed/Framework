namespace Boxed.Mapping.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncMapper : IAsyncMapper<MapFrom, MapTo>
    {
        public CancellationToken CancellationToken { get; private set; }

        public Task MapAsync(MapFrom source, MapTo destination, CancellationToken cancellationToken)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            this.CancellationToken = cancellationToken;
            destination.Property = source.Property;
            return Task.CompletedTask;
        }
    }
}
