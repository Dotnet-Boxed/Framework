namespace Boilerplate.Mapping.Benchmark.Mapping
{
    using AutoMapper;
    using Boilerplate.Mapping.Benchmark.Models;

    public static class AutomapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(
                x => x.CreateMap<MapFrom, MapTo>()
                    .ForMember(y => y.BooleanTo, y => y.MapFrom(z => z.BooleanFrom))
                    .ForMember(y => y.DateTimeOffsetTo, y => y.MapFrom(z => z.DateTimeOffsetFrom))
                    .ForMember(y => y.IntegerTo, y => y.MapFrom(z => z.IntegerFrom))
                    .ForMember(y => y.LongTo, y => y.MapFrom(z => z.LongFrom))
                    .ForMember(y => y.StringTo, y => y.MapFrom(z => z.StringFrom)));
            return configuration.CreateMapper();
        }
    }
}
