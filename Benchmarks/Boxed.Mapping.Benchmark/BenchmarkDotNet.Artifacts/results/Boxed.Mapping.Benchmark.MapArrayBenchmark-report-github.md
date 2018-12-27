``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.472 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.100
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|      Method |  Job | Runtime |      Mean |     Error |    StdDev |      Min |       Max | Ratio | RatioSD | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------ |----- |-------- |----------:|----------:|----------:|---------:|----------:|------:|--------:|------------:|------------:|------------:|--------------------:|
|    Baseline |  Clr |     Clr |  1.204 us | 0.0195 us | 0.0183 us | 1.163 us |  1.229 us |  1.00 |    0.00 |      2.0409 |           - |           - |             6.27 KB |
| BoxedMapper |  Clr |     Clr |  2.637 us | 0.0394 us | 0.0369 us | 2.587 us |  2.695 us |  2.19 |    0.05 |      2.0409 |           - |           - |             6.27 KB |
|  Automapper |  Clr |     Clr | 10.009 us | 0.1876 us | 0.1755 us | 9.720 us | 10.216 us |  8.31 |    0.20 |      2.0447 |           - |           - |              6.3 KB |
|             |      |         |           |           |           |          |           |       |         |             |             |             |                     |
|    Baseline | Core |    Core |  1.281 us | 0.0229 us | 0.0203 us | 1.249 us |  1.316 us |  1.00 |    0.00 |      2.0409 |           - |           - |             6.27 KB |
| BoxedMapper | Core |    Core |  1.986 us | 0.0255 us | 0.0239 us | 1.936 us |  2.013 us |  1.55 |    0.03 |      2.0409 |           - |           - |             6.27 KB |
|  Automapper | Core |    Core |  2.492 us | 0.0478 us | 0.0550 us | 2.395 us |  2.630 us |  1.95 |    0.05 |      2.0485 |           - |           - |              6.3 KB |
