``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531250 Hz, Resolution=395.0617 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host] : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT  [AttachedDebugger]
  Clr    : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2110.0
  Core   : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |            Method |  Job | Runtime |       Mean |     Error |    StdDev |        Min |        Max | Scaled | ScaledSD |  Gen 0 | Allocated |
 |------------------ |----- |-------- |-----------:|----------:|----------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
 |          Baseline |  Clr |     Clr |   6.729 ns | 0.2319 ns | 0.3016 ns |   6.275 ns |   7.060 ns |   1.00 |     0.00 | 0.0178 |      56 B |
 | BoilerplateMapper |  Clr |     Clr |  22.540 ns | 0.1381 ns | 0.1153 ns |  22.366 ns |  22.792 ns |   3.36 |     0.15 | 0.0178 |      56 B |
 |        Automapper |  Clr |     Clr | 251.483 ns | 4.5049 ns | 5.3627 ns | 239.490 ns | 262.602 ns |  37.44 |     1.85 | 0.0176 |      56 B |
 |          Baseline | Core |    Core |   6.287 ns | 0.2182 ns | 0.2143 ns |   5.897 ns |   6.490 ns |   1.00 |     0.00 | 0.0178 |      56 B |
 | BoilerplateMapper | Core |    Core |  14.592 ns | 0.3739 ns | 0.4156 ns |  13.707 ns |  15.259 ns |   2.32 |     0.10 | 0.0178 |      56 B |
 |        Automapper | Core |    Core | 146.027 ns | 2.9206 ns | 3.8990 ns | 142.090 ns | 156.037 ns |  23.25 |     1.00 | 0.0176 |      56 B |
