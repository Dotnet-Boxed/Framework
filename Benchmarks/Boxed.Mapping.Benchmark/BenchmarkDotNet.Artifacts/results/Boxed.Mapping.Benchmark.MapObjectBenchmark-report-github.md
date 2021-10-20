``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Xeon W-2155 CPU 3.30GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]               : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET 5.0             : .NET 5.0.11 (5.0.1121.47308), X64 RyuJIT
  .NET 6.0             : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT


```
|      Method |                  Job |              Runtime |       Mean |     Error |    StdDev |        Min |        Max | Ratio | RatioSD |  Gen 0 | Allocated |
|------------ |--------------------- |--------------------- |-----------:|----------:|----------:|-----------:|-----------:|------:|--------:|-------:|----------:|
|    Baseline |             .NET 5.0 |             .NET 5.0 |   5.918 ns | 0.1840 ns | 0.2191 ns |   5.500 ns |   6.368 ns |  1.00 |    0.00 | 0.0078 |      56 B |
| BoxedMapper |             .NET 5.0 |             .NET 5.0 |  12.239 ns | 0.2974 ns | 0.3183 ns |  11.819 ns |  12.914 ns |  2.08 |    0.08 | 0.0078 |      56 B |
|  Automapper |             .NET 5.0 |             .NET 5.0 | 100.739 ns | 1.8557 ns | 1.7358 ns |  98.590 ns | 104.068 ns | 17.09 |    0.86 | 0.0077 |      56 B |
|             |                      |                      |            |           |           |            |            |       |         |        |           |
|    Baseline |             .NET 6.0 |             .NET 6.0 |   5.847 ns | 0.1912 ns | 0.2680 ns |   5.524 ns |   6.500 ns |  1.00 |    0.00 | 0.0078 |      56 B |
| BoxedMapper |             .NET 6.0 |             .NET 6.0 |  12.930 ns | 0.3269 ns | 0.4475 ns |  12.331 ns |  13.847 ns |  2.22 |    0.12 | 0.0078 |      56 B |
|  Automapper |             .NET 6.0 |             .NET 6.0 |  91.578 ns | 1.1442 ns | 1.0143 ns |  90.415 ns |  93.638 ns | 15.59 |    0.82 | 0.0077 |      56 B |
|             |                      |                      |            |           |           |            |            |       |         |        |           |
|    Baseline |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |         - |
| BoxedMapper |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |         - |
|  Automapper |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |         - |
|             |                      |                      |            |           |           |            |            |       |         |        |           |
|    Baseline | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   5.368 ns | 0.1759 ns | 0.1883 ns |   5.065 ns |   5.766 ns |  1.00 |    0.00 | 0.0089 |      56 B |
| BoxedMapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  17.583 ns | 0.3819 ns | 0.3573 ns |  17.093 ns |  18.326 ns |  3.29 |    0.16 | 0.0089 |      56 B |
|  Automapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 205.491 ns | 3.9671 ns | 4.4094 ns | 199.965 ns | 214.627 ns | 38.31 |    1.55 | 0.0088 |      56 B |

Benchmarks with issues:
  MapObjectBenchmark.Baseline: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapObjectBenchmark.BoxedMapper: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapObjectBenchmark.Automapper: .NET Core 3.0(Runtime=.NET Core 3.0)
