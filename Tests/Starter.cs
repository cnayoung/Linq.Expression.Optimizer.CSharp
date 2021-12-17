using System;

using BenchmarkDotNet.Running;

using Tests;

var r = BenchmarkRunner.Run<UnitTests.Benchmark>();
Console.WriteLine(r);
Console.ReadKey();
return 0;

// BenchmarkDotNet = v0.13.1, OS = Windows 10.0.22000
// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
//    .NET SDK=6.0.100
//    [Host]     : .NET 6.0.0(6.0.21.52210), X64 RyuJIT
// DefaultJob : .NET 6.0.0(6.0.21.52210), X64 RyuJIT


//    |        Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |    Gen 0 |   Gen 1 | Allocated |
//    |-------------- |---------:|---------:|---------:|---------:|------:|--------:|---------:|--------:|----------:|
//    | ExecuteDirect | 33.91 ms | 0.676 ms | 1.917 ms | 33.55 ms |  1.00 |    0.00 | 125.0000 | 62.5000 |    839 KB |
//    |   ExecuteOpt1 | 35.39 ms | 0.924 ms | 2.681 ms | 34.31 ms |  1.05 |    0.10 | 125.0000 | 62.5000 |    978 KB |