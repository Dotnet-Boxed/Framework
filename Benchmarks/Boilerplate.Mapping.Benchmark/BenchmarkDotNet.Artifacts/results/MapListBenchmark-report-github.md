``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531250 Hz, Resolution=395.0617 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host] : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT  [AttachedDebugger]
  Clr    : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2110.0
  Core   : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |            Method |  Job | Runtime |      Mean |     Error |    StdDev |      Min |       Max | Scaled | ScaledSD |  Gen 0 | Allocated |
 |------------------ |----- |-------- |----------:|----------:|----------:|---------:|----------:|-------:|---------:|-------:|----------:|
 |          Baseline |  Clr |     Clr |  1.710 us | 0.0110 us | 0.0086 us | 1.698 us |  1.726 us |   1.00 |     0.00 | 2.0542 |   6.31 KB |
 | BoilerplateMapper |  Clr |     Clr |  2.850 us | 0.0572 us | 0.0535 us | 2.732 us |  2.917 us |   1.67 |     0.03 | 2.0523 |   6.31 KB |
 |        Automapper |  Clr |     Clr | 10.606 us | 0.2095 us | 0.4814 us | 9.719 us | 11.521 us |   6.20 |     0.28 | 2.4719 |   7.62 KB |
 |          Baseline | Core |    Core |  1.613 us | 0.0191 us | 0.0138 us | 1.589 us |  1.634 us |   1.00 |     0.00 | 2.0542 |   6.31 KB |
 | BoilerplateMapper | Core |    Core |  2.294 us | 0.0457 us | 0.0763 us | 2.172 us |  2.518 us |   1.42 |     0.05 | 2.0523 |   6.31 KB |
 |        Automapper | Core |    Core |  3.178 us | 0.0520 us | 0.0487 us | 3.104 us |  3.268 us |   1.97 |     0.03 | 2.4757 |   7.62 KB |
