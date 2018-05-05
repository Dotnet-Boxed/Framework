``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531248 Hz, Resolution=395.0620 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host] : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT  [AttachedDebugger]
  Clr    : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2115.0
  Core   : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |            Method |  Job | Runtime |       Mean |     Error |     StdDev |     Median |        Min |        Max | Scaled | ScaledSD |  Gen 0 | Allocated |
 |------------------ |----- |-------- |-----------:|----------:|-----------:|-----------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
 |          Baseline |  Clr |     Clr |   7.412 ns | 0.3332 ns |  0.6339 ns |   7.121 ns |   6.770 ns |   9.195 ns |   1.00 |     0.00 | 0.0178 |      56 B |
 | BoxedMapper |  Clr |     Clr |  23.986 ns | 0.5647 ns |  0.9891 ns |  23.953 ns |  21.915 ns |  25.805 ns |   3.26 |     0.28 | 0.0178 |      56 B |
 |        Automapper |  Clr |     Clr | 249.089 ns | 5.5299 ns | 10.6543 ns | 249.053 ns | 232.878 ns | 274.881 ns |  33.82 |     2.93 | 0.0176 |      56 B |
 |          Baseline | Core |    Core |   6.330 ns | 0.0883 ns |  0.0737 ns |   6.369 ns |   6.164 ns |   6.391 ns |   1.00 |     0.00 | 0.0178 |      56 B |
 | BoxedMapper | Core |    Core |  15.721 ns | 0.2400 ns |  0.1735 ns |  15.655 ns |  15.523 ns |  16.121 ns |   2.48 |     0.04 | 0.0178 |      56 B |
 |        Automapper | Core |    Core | 134.136 ns | 0.9918 ns |  0.8282 ns | 133.968 ns | 132.698 ns | 135.692 ns |  21.19 |     0.27 | 0.0176 |      56 B |
