using System;
using BenchmarkDotNet.Running;
using Tests;

var r = BenchmarkRunner.Run<UnitTests.Benchmark>();
Console.WriteLine(r);
Console.ReadKey();
return 0;