namespace Boxed.Mapping.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncMapper : IAsyncMapper<MapFrom, MapTo>
    {
        public CancellationToken CancellationToken { get; private set; }

        public Task MapAsync(MapFrom from, MapTo to, CancellationToken cancellationToken)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            this.CancellationToken = cancellationToken;
            to.Property = from.Property;
            return Task.FromResult<object>(null);
        }
    }
}
