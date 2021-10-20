``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Xeon W-2155 CPU 3.30GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]               : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET 5.0             : .NET 5.0.11 (5.0.1121.47308), X64 RyuJIT
  .NET 6.0             : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT


```
|      Method |                  Job |              Runtime |       Mean |     Error |    StdDev |        Min |        Max | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|------------ |--------------------- |--------------------- |-----------:|----------:|----------:|-----------:|-----------:|------:|--------:|-------:|-------:|----------:|
|    Baseline |             .NET 5.0 |             .NET 5.0 |   868.9 ns |  13.56 ns |  12.02 ns |   850.9 ns |   889.9 ns |  1.00 |    0.00 | 0.8936 | 0.0401 |   6,424 B |
| BoxedMapper |             .NET 5.0 |             .NET 5.0 | 1,419.0 ns |  27.37 ns |  32.58 ns | 1,376.4 ns | 1,487.8 ns |  1.63 |    0.05 | 0.8926 | 0.0401 |   6,424 B |
|  Automapper |             .NET 5.0 |             .NET 5.0 | 1,654.2 ns |  32.84 ns |  33.72 ns | 1,606.8 ns | 1,735.2 ns |  1.90 |    0.04 | 0.8926 | 0.0401 |   6,424 B |
|             |                      |                      |            |           |           |            |            |       |         |        |        |           |
|    Baseline |             .NET 6.0 |             .NET 6.0 |   994.6 ns |  19.54 ns |  31.56 ns |   958.2 ns | 1,058.2 ns |  1.00 |    0.00 | 0.8926 | 0.0401 |   6,424 B |
| BoxedMapper |             .NET 6.0 |             .NET 6.0 | 1,483.3 ns |  17.53 ns |  15.54 ns | 1,465.1 ns | 1,517.6 ns |  1.48 |    0.05 | 0.8926 | 0.0401 |   6,424 B |
|  Automapper |             .NET 6.0 |             .NET 6.0 | 1,613.8 ns |  31.32 ns |  27.76 ns | 1,583.0 ns | 1,666.1 ns |  1.61 |    0.06 | 0.8926 | 0.0401 |   6,424 B |
|             |                      |                      |            |           |           |            |            |       |         |        |        |           |
|    Baseline |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |      - |         - |
| BoxedMapper |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |      - |         - |
|  Automapper |        .NET Core 3.0 |        .NET Core 3.0 |         NA |        NA |        NA |         NA |         NA |     ? |       ? |      - |      - |         - |
|             |                      |                      |            |           |           |            |            |       |         |        |        |           |
|    Baseline | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   848.6 ns |  16.69 ns |  15.62 ns |   830.4 ns |   875.2 ns |  1.00 |    0.00 | 1.0233 | 0.0420 |   6,443 B |
| BoxedMapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 1,953.0 ns |  21.18 ns |  18.77 ns | 1,929.8 ns | 1,985.1 ns |  2.31 |    0.04 | 1.0223 | 0.0420 |   6,443 B |
|  Automapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 6,993.6 ns | 139.22 ns | 160.32 ns | 6,820.6 ns | 7,413.7 ns |  8.26 |    0.20 | 1.0223 | 0.0381 |   6,443 B |

Benchmarks with issues:
  MapArrayBenchmark.Baseline: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapArrayBenchmark.BoxedMapper: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapArrayBenchmark.Automapper: .NET Core 3.0(Runtime=.NET Core 3.0)
