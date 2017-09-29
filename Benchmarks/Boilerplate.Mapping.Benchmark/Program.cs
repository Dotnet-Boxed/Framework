namespace Boilerplate.Mapping.Benchmark
{
    using System;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            // BenchmarkRunner.Run<MapObjectBenchmark>();
            BenchmarkRunner.Run<MapArrayBenchmark>();
            // BenchmarkRunner.Run<MapListBenchmark>();
            Console.Read();
        }
    }
}
