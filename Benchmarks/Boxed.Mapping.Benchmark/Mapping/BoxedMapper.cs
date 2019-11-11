namespace Boxed.Mapping.Benchmark.Mapping
{
    using System;
    using Boxed.Mapping.Benchmark.Models;

    public class BoxedMapper : IMapper<MapFrom, MapTo>
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

            destination.BooleanTo = source.BooleanFrom;
            destination.DateTimeOffsetTo = source.DateTimeOffsetFrom;
            destination.IntegerTo = source.IntegerFrom;
            destination.LongTo = source.LongFrom;
            destination.StringTo = source.StringFrom;
        }
    }
}
