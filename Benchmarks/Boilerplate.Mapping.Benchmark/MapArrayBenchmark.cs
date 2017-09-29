namespace Boilerplate.Mapping.Benchmark
{
    using System;
    using AutoMapper;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    using BenchmarkDotNet.Attributes.Jobs;

    // [KeepBenchmarkFiles]
    [ClrJob]
    [CoreJob]
    [MinColumn]
    [MaxColumn]
    [HtmlExporter]
    [CsvExporter]
    [RPlotExporter]
    [MemoryDiagnoser]
    public class MapArrayBenchmark
    {
        private readonly IMapper automapper;
        private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
        private readonly Random random;
        private MapFrom[] mapFrom;

        public MapArrayBenchmark()
        {
            var configuration = new MapperConfiguration(
                x => x.CreateMap<MapFrom, MapTo>()
                    .ForMember(y => y.BooleanTo, y => y.MapFrom(z => z.BooleanFrom))
                    .ForMember(y => y.DateTimeOffsetTo, y => y.MapFrom(z => z.DateTimeOffsetFrom))
                    .ForMember(y => y.IntegerTo, y => y.MapFrom(z => z.IntegerFrom))
                    .ForMember(y => y.LongTo, y => y.MapFrom(z => z.LongFrom))
                    .ForMember(y => y.StringTo, y => y.MapFrom(z => z.StringFrom)));
            this.automapper = configuration.CreateMapper();
            this.boilerplateMapper = new BoilerplateMapper();
            this.random = new Random();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.mapFrom = new MapFrom[100];
            for (int i = 0; i < this.mapFrom.Length; ++i)
            {
                this.mapFrom[i] = new MapFrom()
                {
                    BooleanFrom = this.random.NextDouble() > 0.5D,
                    DateTimeOffsetFrom = DateTimeOffset.UtcNow,
                    IntegerFrom = this.random.Next(),
                    LongFrom = this.random.Next(),
                    StringFrom = this.random.Next().ToString()
                };
            }
        }

        [Benchmark(Baseline = true)]
        public MapTo[] Baseline()
        {
            var destination = new MapTo[this.mapFrom.Length];
            for (int i = 0; i < this.mapFrom.Length; ++i)
            {
                var item = this.mapFrom[i];
                destination[i] = new MapTo()
                {
                    BooleanTo = item.BooleanFrom,
                    DateTimeOffsetTo = item.DateTimeOffsetFrom,
                    IntegerTo = item.IntegerFrom,
                    LongTo = item.LongFrom,
                    StringTo = item.StringFrom
                };
            }

            return destination;
        }

        [Benchmark]
        public MapTo[] BoilerplateMapper() => this.boilerplateMapper.MapArray(this.mapFrom);

        [Benchmark]
        public MapTo[] Automapper() => this.automapper.Map<MapTo[]>(this.mapFrom);
    }
}
