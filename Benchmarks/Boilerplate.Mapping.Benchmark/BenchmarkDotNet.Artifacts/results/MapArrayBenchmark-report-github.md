``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531248 Hz, Resolution=395.0620 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host] : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT  [AttachedDebugger]
  Clr    : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2115.0
  Core   : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |            Method |  Job | Runtime |      Mean |     Error |    StdDev |       Min |       Max | Scaled | ScaledSD |  Gen 0 | Allocated |
 |------------------ |----- |-------- |----------:|----------:|----------:|----------:|----------:|-------:|---------:|-------:|----------:|
 |          Baseline |  Clr |     Clr |  1.083 us | 0.0095 us | 0.0079 us |  1.066 us |  1.095 us |   1.00 |     0.00 | 2.0409 |   6.27 KB |
 | BoilerplateMapper |  Clr |     Clr |  3.234 us | 0.0779 us | 0.2273 us |  2.665 us |  3.744 us |   2.99 |     0.21 | 2.0409 |   6.27 KB |
 |        Automapper |  Clr |     Clr | 11.498 us | 0.2297 us | 0.6664 us | 10.380 us | 13.136 us |  10.62 |     0.62 | 2.0294 |   6.27 KB |
 |          Baseline | Core |    Core |  1.367 us | 0.0271 us | 0.0654 us |  1.234 us |  1.516 us |   1.00 |     0.00 | 2.0409 |   6.27 KB |
 | BoilerplateMapper | Core |    Core |  1.995 us | 0.0400 us | 0.0966 us |  1.855 us |  2.286 us |   1.46 |     0.10 | 2.0409 |   6.27 KB |
 |        Automapper | Core |    Core |  2.561 us | 0.0510 us | 0.0824 us |  2.423 us |  2.702 us |   1.88 |     0.11 | 2.0409 |   6.27 KB |
