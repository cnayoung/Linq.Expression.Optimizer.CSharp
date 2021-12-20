using System;

using BenchmarkDotNet.Running;

using Tests;

var r = BenchmarkRunner.Run<UnitTests.Benchmark>();
Console.WriteLine(r);
Console.ReadKey();
//var ut = new UnitTests.Benchmark();
//for (var i = 0; i < 100; i++) {
//    ut.VisitOpt1();
//}
return 0;

// BenchmarkDotNet = v0.13.1, OS = Windows 10.0.22000
// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
//    .NET SDK=6.0.100
//    [Host]     : .NET 6.0.0(6.0.21.52210), X64 RyuJIT
// DefaultJob : .NET 6.0.0(6.0.21.52210), X64 RyuJIT


//|        Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |    Gen 0 |   Gen 1 | Allocated |
//|-------------- |----------:|----------:|----------:|----------:|------:|--------:|---------:|--------:|----------:|
//| ExecuteDirect | 25.590 ms | 0.7128 ms | 2.1017 ms | 24.787 ms |  1.00 |    0.00 | 125.0000 | 62.5000 |    831 KB |
//|   ExecuteOpt1 | 25.053 ms | 0.4991 ms | 0.6312 ms | 25.192 ms |  0.99 |    0.06 | 156.2500 | 62.5000 |    960 KB |
//|     VisitOpt1 |  1.026 ms | 0.0162 ms | 0.0144 ms |  1.029 ms |  0.04 |    0.00 |  19.5313 |  3.9063 |    126 KB |