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
    public class MapObjectBenchmark
    {
        private readonly IMapper automapper;
        private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
        private readonly Random random;
        private MapFrom mapFrom;

        public MapObjectBenchmark()
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
            this.mapFrom = new MapFrom()
            {
                BooleanFrom = this.random.NextDouble() > 0.5D,
                DateTimeOffsetFrom = DateTimeOffset.UtcNow,
                IntegerFrom = this.random.Next(),
                LongFrom = this.random.Next(),
                StringFrom = this.random.Next().ToString()
            };
        }

        [Benchmark(Baseline = true)]
        public MapTo Baseline() => new MapTo()
        {
            BooleanTo = this.mapFrom.BooleanFrom,
            DateTimeOffsetTo = this.mapFrom.DateTimeOffsetFrom,
            IntegerTo = this.mapFrom.IntegerFrom,
            LongTo = this.mapFrom.LongFrom,
            StringTo = this.mapFrom.StringFrom
        };

        [Benchmark]
        public MapTo BoilerplateMapper() => this.boilerplateMapper.Map(this.mapFrom);

        [Benchmark]
        public MapTo Automapper() => this.automapper.Map<MapTo>(this.mapFrom);
    }
}
