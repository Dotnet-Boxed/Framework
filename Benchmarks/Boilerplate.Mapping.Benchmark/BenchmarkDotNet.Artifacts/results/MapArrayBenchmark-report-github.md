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
 |          Baseline |  Clr |     Clr |  1.050 us | 0.0209 us | 0.0312 us | 1.014 us |  1.120 us |   1.00 |     0.00 | 2.0409 |   6.27 KB |
 | BoilerplateMapper |  Clr |     Clr |  2.491 us | 0.0204 us | 0.0171 us | 2.458 us |  2.515 us |   2.37 |     0.07 | 2.0409 |   6.27 KB |
 |        Automapper |  Clr |     Clr | 10.028 us | 0.2161 us | 0.2021 us | 9.820 us | 10.471 us |   9.56 |     0.33 | 2.0294 |   6.27 KB |
 |          Baseline | Core |    Core |  1.053 us | 0.0200 us | 0.0206 us | 1.023 us |  1.087 us |   1.00 |     0.00 | 2.0409 |   6.27 KB |
 | BoilerplateMapper | Core |    Core |  1.786 us | 0.0348 us | 0.0510 us | 1.727 us |  1.919 us |   1.70 |     0.06 | 2.0409 |   6.27 KB |
 |        Automapper | Core |    Core |  2.428 us | 0.0485 us | 0.0558 us | 2.304 us |  2.572 us |   2.31 |     0.07 | 2.0409 |   6.27 KB |
