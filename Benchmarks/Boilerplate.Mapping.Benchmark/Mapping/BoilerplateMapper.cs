namespace Boilerplate.Mapping.Benchmark.Mapping
{
    using Boilerplate.Mapping.Benchmark.Models;

    public class BoilerplateMapper : IMapper<MapFrom, MapTo>
    {
        public void Map(MapFrom source, MapTo destination)
        {
            destination.BooleanTo = source.BooleanFrom;
            destination.DateTimeOffsetTo = source.DateTimeOffsetFrom;
            destination.IntegerTo = source.IntegerFrom;
            destination.LongTo = source.LongFrom;
            destination.StringTo = source.StringFrom;
        }
    }
}
