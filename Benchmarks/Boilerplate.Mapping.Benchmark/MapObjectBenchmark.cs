namespace Boilerplate.Mapping.Benchmark
{
    using System;
    using AutoMapper;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    using BenchmarkDotNet.Attributes.Jobs;
    using Boilerplate.Mapping.Benchmark.Mapping;
    using Boilerplate.Mapping.Benchmark.Models;

    // [KeepBenchmarkFiles]
    [ClrJob]
    [CoreJob]
    [MinColumn]
    [MaxColumn]
    [HtmlExporter]
    [CsvExporter]
    [CsvMeasurementsExporter]
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
            this.automapper = AutomapperConfiguration.CreateMapper();
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
