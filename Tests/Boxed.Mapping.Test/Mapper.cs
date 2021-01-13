namespace Boxed.Mapping.Test
{
    using System;

    public class Mapper : IMapper<MapFrom, MapTo>
    {
        public void Map(MapFrom source, MapTo destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Property = source.Property;
        }
    }
}
