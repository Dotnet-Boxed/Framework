namespace Boxed.Mapping.Test
{
    public class Mapper : IMapper<MapFrom, MapTo>
    {
        public void Map(MapFrom from, MapTo to)
        {
            to.Property = from.Property;
        }
    }
}
