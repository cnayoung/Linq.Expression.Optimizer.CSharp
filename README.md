# Linq.Expression.Optimizer.CSharp
A C# port of Thorium's F# optimizer code.

Tuomas Hietanen's code is written in F# and can be found here: https://github.com/Thorium/Linq.Expression.Optimizer.  The code provides a lightweight optimizer for System.Linq.Expression expressions. It supports basic Boolean algebra and reductions and constant and tuple/anonymous type eliminations. It is designed for side-effect free Expressions. There are no compilation-subjective optimizations. The code is meant to be used with expressions that are not compiled but transferred to other domains.

For further information, please see the documentation and tutorial available on the Thorium site.

I needed a starting point for Linq expression optimisation for a project I'm working on, and Tuomas' code meets that need. However, the project uses C# and I didn't want part of the code base to be in a different language. So, as a starting point, I ported the F# code into C# 10. It's a direct port and is keeps as close as possible to the original.  I've even kept the comments!  Unlike the F# version, I've only provided support for .NET 6.0.  Of course, C# is not a functional language, and you have to hold the compiler's hand a little more tightly than F#.  Pattern matching isn't identical, and C# has its own 'built-in' alternative to Option. However, as a testament to how far C# has evolved over the years, there is broadly a one-to-one correspondence with the F# code.

The F# code is public domain, so I have licenced this version of the code on the same terms.  I have not provided a nuget package because I can't see the point in having two packages with identical function.

## Unit and Benchmark Tests
I've provided a set of tests based on Tuomas' tests. There are some issues, though. One of the tests (for query 11) is broken in the F# version, and was similarly broken here, but I've amended the test to pass. 

The original code uses F# query expressions. For the C# version, I've used C# query syntax which can generate slightly different expressions.  In addition, the C# compiler has a handful of built-in optimisations which automatically optimise the query expressions at the compiler level.  These are nowhere as extensive as the optimisations implemented here, but they overlap.  To better test the code, I constructed some query expressions directly, rather than using query syntax.

The biggest problem is that for 8 of the 16 queries, the code is broken, even though the corresponding unit tests pass.  For these queries, the F# query expressions generate Linq expression code to represent a SelectEnumerableIterator. This class is used to perform lazy evaluation of an underlying Linq query when the code iterates through the results.  The optimizer contains a function named ``WhereSelectEnumerableIterator visitor`` which attempts to handle these cases.  However, the code does not work on the current version of F# (F# 6.0).  There are two problems.  First, the code attempts to reflect on two private fields in the iterator using incorrect names (“source” and “expression”).  These field names are actually prefixed by an underscore "\_source” and “\_expression”).  It may be that these names have been changed in recent versions of F#, and were not prefixed when the optimizer code was originally written.  That’s the trouble with reflecting on private members.  Things break easily.

The second problem is not exactly a bug, but it is an issue if you attempt to compare the performance of the F# and C# benchmark tests.  If you fix the first issue, the code changes the inner expression within the iterator.  This is exactly the right thing to do, given the behaviour of F# query expressions.  The visitor hands back the optimised iterator.  In the benchmark tests, the code then executes the iterator expression.  However, executing a lazy-evaluated iterator does not actually iterate through the collection, and so very little work is done.

The behaviour of the C# code is quite different.  When you build an IQueryable\<T\> Linq expression, you get the actual expression itself, rather than a lazy evaluator.  That is because IQueryable\<T\> is all about supporting Linq providers, and the last thing the C# compiler should do is second-guess the strategy adopted by a provider by assuming lazy evaluation.  How would that work against a database, for example?  IQueryable\<T\> needs to provide the full query expression and let the provider figure what to do with it.  Providers should certainly not have to reflect on private members to get the full query expression!  
  
I’m not familiar enough with the thinking behind F#’s query expressions to criticise its implementation, but I can certainly say that F# query expressions are not Linq provider-friendly, which rather begs the question, in my mind, why they return an IQueryable\<T\>.
  
In the C# code, the benchmark tests get the full query expressions for all the queries which they then execute.  When you run the benchmark tests for the C# code, you will find they take roughly twice as long. This is because most of the work is in executing the Linq expressions.  The C# tests do twice the amount of work as the F# tests.  If you eliminate the problem queries, or if you amend the F# code to replace the SelectEnumerableIterator expressions with the full query expressions, the benchmark figures are nearly identical for the ‘visit and execute’ test, as you might expect.

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT DEBUG
    DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
    
F#
**
|      Method |     Mean |    Error |   StdDev |   Median |    Gen 0 | Allocated |
|------------ |---------:|---------:|---------:|---------:|---------:|----------:|
| ExecuteOpt1 | 34.08 ms | 2.069 ms | 5.969 ms | 32.12 ms | 100.0000 |    941 KB |

C#
**
|        Method |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen 0 |   Gen 1 | Allocated |
|-------------- |---------:|---------:|---------:|------:|--------:|---------:|--------:|----------:|
|   ExecuteOpt1 | 35.83 ms | 0.674 ms | 1.392 ms |  1.02 |    0.07 | 133.3333 | 66.6667 |    977 KB |
