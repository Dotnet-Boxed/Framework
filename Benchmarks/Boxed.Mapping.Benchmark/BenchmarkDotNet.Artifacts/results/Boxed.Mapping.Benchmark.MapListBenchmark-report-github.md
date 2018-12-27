``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.472 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|      Method |  Job | Runtime |      Mean |     Error |    StdDev |       Min |       Max | Ratio | RatioSD | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------ |----- |-------- |----------:|----------:|----------:|----------:|----------:|------:|--------:|------------:|------------:|------------:|--------------------:|
|    Baseline |  Clr |     Clr |  1.833 us | 0.0229 us | 0.0214 us |  1.791 us |  1.853 us |  1.00 |    0.00 |      2.0542 |           - |           - |             6.31 KB |
| BoxedMapper |  Clr |     Clr |  3.295 us | 0.0404 us | 0.0378 us |  3.241 us |  3.348 us |  1.80 |    0.03 |      2.0523 |           - |           - |             6.31 KB |
|  Automapper |  Clr |     Clr | 10.569 us | 0.2024 us | 0.1893 us | 10.232 us | 10.867 us |  5.77 |    0.13 |      2.4872 |           - |           - |             7.65 KB |
|             |      |         |           |           |           |           |           |       |         |             |             |             |                     |
|    Baseline | Core |    Core |  1.735 us | 0.0118 us | 0.0110 us |  1.713 us |  1.751 us |  1.00 |    0.00 |      2.0542 |           - |           - |             6.31 KB |
| BoxedMapper | Core |    Core |  2.237 us | 0.0426 us | 0.0398 us |  2.178 us |  2.288 us |  1.29 |    0.03 |      2.0523 |           - |           - |             6.31 KB |
|  Automapper | Core |    Core |  3.220 us | 0.0491 us | 0.0459 us |  3.138 us |  3.283 us |  1.86 |    0.03 |      2.4872 |           - |           - |             7.65 KB |
