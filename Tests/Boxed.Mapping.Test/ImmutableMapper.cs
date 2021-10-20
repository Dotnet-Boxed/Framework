namespace Boxed.Mapping.Test
{
    using System;

    public class ImmutableMapper : IImmutableMapper<MapFrom, MapTo>
    {
        public MapTo Map(MapFrom source)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(source);
#else
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
#endif
            return new MapTo()
            {
                Property = source.Property,
            };
        }
    }
}
