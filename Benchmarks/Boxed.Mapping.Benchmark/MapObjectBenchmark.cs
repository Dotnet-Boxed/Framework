namespace Boxed.Mapping.Benchmark;

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
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net50)]
[SimpleJob(RuntimeMoniker.NetCoreApp30)]
public class MapObjectBenchmark
{
    private readonly IMapper automapper;
    private readonly IMapper<MapFrom, MapTo> boilerplateMapper;
    private readonly Random random;
    private MapFrom mapFrom = default!;

    public MapObjectBenchmark()
    {
        this.automapper = AutomapperConfiguration.CreateMapper();
        this.boilerplateMapper = new BoxedMapper();
        this.random = new Random();
    }

    [GlobalSetup]
    public void GlobalSetup() =>
        this.mapFrom = new MapFrom()
        {
#pragma warning disable CA5394 // Do not use insecure randomness
            BooleanFrom = this.random.NextDouble() > 0.5D,
            DateTimeOffsetFrom = DateTimeOffset.UtcNow,
            IntegerFrom = this.random.Next(),
            LongFrom = this.random.Next(),
            StringFrom = this.random.Next().ToString(CultureInfo.InvariantCulture),
#pragma warning restore CA5394 // Do not use insecure randomness
        };

    [Benchmark(Baseline = true)]
    public MapTo Baseline() => new()
    {
        BooleanTo = this.mapFrom.BooleanFrom,
        DateTimeOffsetTo = this.mapFrom.DateTimeOffsetFrom,
        IntegerTo = this.mapFrom.IntegerFrom,
        LongTo = this.mapFrom.LongFrom,
        StringTo = this.mapFrom.StringFrom,
    };

    [Benchmark]
    public MapTo BoxedMapper() => this.boilerplateMapper.Map(this.mapFrom);

    [Benchmark]
    public MapTo Automapper() => this.automapper.Map<MapTo>(this.mapFrom);
}
