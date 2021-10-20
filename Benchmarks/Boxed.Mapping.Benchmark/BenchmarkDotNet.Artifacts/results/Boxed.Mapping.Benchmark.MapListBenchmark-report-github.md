``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Xeon W-2155 CPU 3.30GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]               : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET 5.0             : .NET 5.0.11 (5.0.1121.47308), X64 RyuJIT
  .NET 6.0             : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT


```
|      Method |                  Job |              Runtime |     Mean |     Error |    StdDev |      Min |      Max | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|------------ |--------------------- |--------------------- |---------:|----------:|----------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|
|    Baseline |             .NET 5.0 |             .NET 5.0 | 1.172 μs | 0.0193 μs | 0.0214 μs | 1.144 μs | 1.212 μs |  1.00 |    0.00 | 0.8965 | 0.0401 |   6,456 B |
| BoxedMapper |             .NET 5.0 |             .NET 5.0 | 1.618 μs | 0.0319 μs | 0.0341 μs | 1.574 μs | 1.675 μs |  1.38 |    0.04 | 0.8965 | 0.0401 |   6,456 B |
|  Automapper |             .NET 5.0 |             .NET 5.0 | 2.118 μs | 0.0414 μs | 0.0387 μs | 2.081 μs | 2.206 μs |  1.80 |    0.04 | 1.0834 | 0.0496 |   7,792 B |
|             |                      |                      |          |           |           |          |          |       |         |        |        |           |
|    Baseline |             .NET 6.0 |             .NET 6.0 | 1.090 μs | 0.0218 μs | 0.0291 μs | 1.044 μs | 1.149 μs |  1.00 |    0.00 | 0.8965 | 0.0401 |   6,456 B |
| BoxedMapper |             .NET 6.0 |             .NET 6.0 | 1.736 μs | 0.0278 μs | 0.0321 μs | 1.705 μs | 1.804 μs |  1.59 |    0.06 | 0.8965 | 0.0401 |   6,456 B |
|  Automapper |             .NET 6.0 |             .NET 6.0 | 1.969 μs | 0.0391 μs | 0.0561 μs | 1.887 μs | 2.096 μs |  1.81 |    0.07 | 1.0834 | 0.0496 |   7,792 B |
|             |                      |                      |          |           |           |          |          |       |         |        |        |           |
|    Baseline |        .NET Core 3.0 |        .NET Core 3.0 |       NA |        NA |        NA |       NA |       NA |     ? |       ? |      - |      - |         - |
| BoxedMapper |        .NET Core 3.0 |        .NET Core 3.0 |       NA |        NA |        NA |       NA |       NA |     ? |       ? |      - |      - |         - |
|  Automapper |        .NET Core 3.0 |        .NET Core 3.0 |       NA |        NA |        NA |       NA |       NA |     ? |       ? |      - |      - |         - |
|             |                      |                      |          |           |           |          |          |       |         |        |        |           |
|    Baseline | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 1.284 μs | 0.0237 μs | 0.0198 μs | 1.258 μs | 1.323 μs |  1.00 |    0.00 | 1.0300 | 0.0420 |   6,483 B |
| BoxedMapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 2.229 μs | 0.0443 μs | 0.0435 μs | 2.181 μs | 2.304 μs |  1.73 |    0.05 | 1.0262 | 0.0420 |   6,483 B |
|  Automapper | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 7.162 μs | 0.0949 μs | 0.0841 μs | 7.044 μs | 7.334 μs |  5.58 |    0.09 | 1.2360 | 0.0534 |   7,823 B |

Benchmarks with issues:
  MapListBenchmark.Baseline: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapListBenchmark.BoxedMapper: .NET Core 3.0(Runtime=.NET Core 3.0)
  MapListBenchmark.Automapper: .NET Core 3.0(Runtime=.NET Core 3.0)
