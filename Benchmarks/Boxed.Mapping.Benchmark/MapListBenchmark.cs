namespace Boxed.Mapping.Benchmark
{
    using System;
    using System.Collections.Generic;
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
    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    public class MapListBenchmark
    {
        private readonly IMapper automapper;
        private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
        private readonly Random random;
        private List<MapFrom> mapFrom;

        public MapListBenchmark()
        {
            this.automapper = AutomapperConfiguration.CreateMapper();
            this.boilerplateMapper = new BoxedMapper();
            this.random = new Random();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.mapFrom = new List<MapFrom>();
            for (var i = 0; i < 100; ++i)
            {
                this.mapFrom.Add(
                    new MapFrom()
                    {
                        BooleanFrom = this.random.NextDouble() > 0.5D,
                        DateTimeOffsetFrom = DateTimeOffset.UtcNow,
                        IntegerFrom = this.random.Next(),
                        LongFrom = this.random.Next(),
                        StringFrom = this.random.Next().ToString(CultureInfo.InvariantCulture),
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
                    StringTo = item.StringFrom,
                });
            }

            return destination;
        }

        [Benchmark]
        public List<MapTo> BoxedMapper() => this.boilerplateMapper.MapList(this.mapFrom);

        [Benchmark]
        public List<MapTo> Automapper() => this.automapper.Map<List<MapTo>>(this.mapFrom);
    }
}
