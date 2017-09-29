namespace Boilerplate.Mapping.Benchmark
{
    using System;
    using System.Collections.Generic;
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
    public class MapListBenchmark
    {
        private readonly IMapper automapper;
        private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
        private readonly Random random;
        private List<MapFrom> mapFrom;

        public MapListBenchmark()
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
            this.mapFrom = new List<MapFrom>();
            for (int i = 0; i < 100; ++i)
            {
                this.mapFrom.Add(
                    new MapFrom()
                    {
                        BooleanFrom = this.random.NextDouble() > 0.5D,
                        DateTimeOffsetFrom = DateTimeOffset.UtcNow,
                        IntegerFrom = this.random.Next(),
                        LongFrom = this.random.Next(),
                        StringFrom = this.random.Next().ToString()
                    });
            }
        }

        [Benchmark(Baseline = true)]
        public List<MapTo> Baseline()
        {
            var destination = new List<MapTo>(this.mapFrom.Count);
            foreach (var item in this.mapFrom)
            {
                destination.Add(new MapTo()
                {
                    BooleanTo = item.BooleanFrom,
                    DateTimeOffsetTo = item.DateTimeOffsetFrom,
                    IntegerTo = item.IntegerFrom,
                    LongTo = item.LongFrom,
                    StringTo = item.StringFrom
                });
            }

            return destination;
        }

        [Benchmark]
        public List<MapTo> BoilerplateMapper() => this.boilerplateMapper.MapList(this.mapFrom);

        [Benchmark]
        public List<MapTo> Automapper() => this.automapper.Map<List<MapTo>>(this.mapFrom);
    }
}
