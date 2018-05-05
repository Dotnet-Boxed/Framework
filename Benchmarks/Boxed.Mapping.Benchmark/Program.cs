namespace Boxed.Mapping.Benchmark
{
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<MapObjectBenchmark>();
            BenchmarkRunner.Run<MapArrayBenchmark>();
            BenchmarkRunner.Run<MapListBenchmark>();
        }
    }
}
