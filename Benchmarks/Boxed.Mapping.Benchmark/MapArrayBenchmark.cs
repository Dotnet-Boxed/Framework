namespace Boxed.Mapping.Benchmark
{
    using System;
    using AutoMapper;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    using BenchmarkDotNet.Attributes.Jobs;
    using Boxed.Mapping.Benchmark.Mapping;
    using Boxed.Mapping.Benchmark.Models;

    // [KeepBenchmarkFiles]
    [ClrJob]
    [CoreJob]
    [MinColumn]
    [MaxColumn]
    [HtmlExporter]
    [CsvMeasurementsExporter]
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
            this.automapper = AutomapperConfiguration.CreateMapper();
            this.boilerplateMapper = new BoxedMapper();
            this.random = new Random();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.mapFrom = new MapFrom[100];
            for (var i = 0; i < this.mapFrom.Length; ++i)
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
            for (var i = 0; i < this.mapFrom.Length; ++i)
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
        public MapTo[] BoxedMapper() => this.boilerplateMapper.MapArray(this.mapFrom);

        [Benchmark]
        public MapTo[] Automapper() => this.automapper.Map<MapTo[]>(this.mapFrom);
    }
}
