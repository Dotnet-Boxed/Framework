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
 |          Baseline |  Clr |     Clr |  1.812 us | 0.0312 us | 0.0277 us |  1.773 us |  1.859 us |   1.00 |     0.00 | 2.0542 |   6.31 KB |
 | BoxedMapper |  Clr |     Clr |  3.107 us | 0.0606 us | 0.0869 us |  2.960 us |  3.239 us |   1.72 |     0.05 | 2.0523 |   6.31 KB |
 |        Automapper |  Clr |     Clr | 11.080 us | 0.2111 us | 0.2513 us | 10.629 us | 11.509 us |   6.12 |     0.16 | 2.4719 |   7.62 KB |
 |          Baseline | Core |    Core |  1.610 us | 0.0315 us | 0.0409 us |  1.548 us |  1.703 us |   1.00 |     0.00 | 2.0542 |   6.31 KB |
 | BoxedMapper | Core |    Core |  2.206 us | 0.0436 us | 0.0680 us |  2.081 us |  2.349 us |   1.37 |     0.05 | 2.0523 |   6.31 KB |
 |        Automapper | Core |    Core |  3.181 us | 0.0621 us | 0.1003 us |  3.041 us |  3.439 us |   1.98 |     0.08 | 2.4757 |   7.62 KB |
