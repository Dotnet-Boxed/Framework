namespace Boxed.Mapping.Benchmark;

using BenchmarkDotNet.Running;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<MapObjectBenchmark>();
        BenchmarkRunner.Run<MapArrayBenchmark>();
        BenchmarkRunner.Run<MapListBenchmark>();
    }
}
