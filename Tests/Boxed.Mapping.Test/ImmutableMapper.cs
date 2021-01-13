namespace Boxed.Mapping.Test
{
    using System;

    public class ImmutableMapper : IImmutableMapper<MapFrom, MapTo>
    {
        public MapTo Map(MapFrom source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new MapTo()
            {
                Property = source.Property,
            };
        }
    }
}
