namespace Boxed.Mapping.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncImmutableMapper : IAsyncImmutableMapper<MapFrom, MapTo>
    {
        public CancellationToken CancellationToken { get; private set; }

        public Task<MapTo> MapAsync(MapFrom from, CancellationToken cancellationToken)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            this.CancellationToken = cancellationToken;
            return Task.FromResult(
                new MapTo()
                {
                    Property = from.Property,
                });
        }
    }
}
