namespace Boxed.Mapping.Test;

using System;

public class Mapper : IMapper<MapFrom, MapTo>
{
    public void Map(MapFrom source, MapTo destination)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);
#else
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }
#endif

        destination.Property = source.Property;
    }
}
