namespace Boxed.Mapping.Test;

using System;
using System.Threading;
using System.Threading.Tasks;

public class AsyncImmutableMapper : IAsyncImmutableMapper<MapFrom, MapTo>
{
    public CancellationToken CancellationToken { get; private set; }

    public Task<MapTo> MapAsync(MapFrom source, CancellationToken cancellationToken)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(source);
#else
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        this.CancellationToken = cancellationToken;
        return Task.FromResult(
            new MapTo()
            {
                Property = source.Property,
            });
    }
}
