``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.472 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|      Method |  Job | Runtime |       Mean |     Error |    StdDev |        Min |        Max | Ratio | RatioSD | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------ |----- |-------- |-----------:|----------:|----------:|-----------:|-----------:|------:|--------:|------------:|------------:|------------:|--------------------:|
|    Baseline |  Clr |     Clr |   7.877 ns | 0.3434 ns | 0.7244 ns |   7.062 ns |   9.937 ns |  1.00 |    0.00 |      0.0178 |           - |           - |                56 B |
| BoxedMapper |  Clr |     Clr |  25.431 ns | 0.4690 ns | 0.4387 ns |  24.923 ns |  26.270 ns |  3.07 |    0.32 |      0.0178 |           - |           - |                56 B |
|  Automapper |  Clr |     Clr | 264.934 ns | 3.9174 ns | 3.6644 ns | 259.375 ns | 271.946 ns | 31.97 |    3.24 |      0.0277 |           - |           - |                88 B |
|             |      |         |            |           |           |            |            |       |         |             |             |             |                     |
|    Baseline | Core |    Core |   9.327 ns | 0.1651 ns | 0.1544 ns |   8.884 ns |   9.521 ns |  1.00 |    0.00 |      0.0178 |           - |           - |                56 B |
| BoxedMapper | Core |    Core |  17.174 ns | 0.3719 ns | 0.3479 ns |  16.412 ns |  17.462 ns |  1.84 |    0.05 |      0.0178 |           - |           - |                56 B |
|  Automapper | Core |    Core | 158.218 ns | 2.9366 ns | 2.7469 ns | 153.464 ns | 162.450 ns | 16.97 |    0.47 |      0.0279 |           - |           - |                88 B |
