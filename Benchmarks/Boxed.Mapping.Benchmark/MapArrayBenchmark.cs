namespace Boxed.Mapping.Benchmark
{
    using System;
    using System.Globalization;
    using AutoMapper;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using Boxed.Mapping.Benchmark.Mapping;
    using Boxed.Mapping.Benchmark.Models;

    [KeepBenchmarkFiles]
    [MemoryDiagnoser]
    [MinColumn]
    [MaxColumn]
    [HtmlExporter]
    [CsvMeasurementsExporter]
    [RPlotExporter]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(RuntimeMoniker.Net472)]
    public class MapArrayBenchmark
    {
        private readonly IMapper automapper;
        private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
        private readonly Random random;
        private MapFrom[] mapFrom = default!;

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
#pragma warning disable CA5394 // Do not use insecure randomness
                    BooleanFrom = this.random.NextDouble() > 0.5D,
                    DateTimeOffsetFrom = DateTimeOffset.UtcNow,
                    IntegerFrom = this.random.Next(),
                    LongFrom = this.random.Next(),
                    StringFrom = this.random.Next().ToString(CultureInfo.InvariantCulture),
#pragma warning restore CA5394 // Do not use insecure randomness
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
                    StringTo = item.StringFrom,
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
