namespace Boxed.Mapping.Test
{
    using System;

    public class ImmutableMapper : IImmutableMapper<MapFrom, MapTo>
    {
        public MapTo Map(MapFrom from)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            return new MapTo()
            {
                Property = from.Property,
            };
        }
    }
}
