namespace Boxed.Mapping.Test
{
    using System;

    public class Mapper : IMapper<MapFrom, MapTo>
    {
        public void Map(MapFrom from, MapTo to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            to.Property = from.Property;
        }
    }
}
