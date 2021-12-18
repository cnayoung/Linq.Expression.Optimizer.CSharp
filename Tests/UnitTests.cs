using System.Linq.Expressions;
using Xunit;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using ExpressionOptimizer;

namespace Tests;

public class UnitTests {
    [Fact]
    public static void ExpressionOptimizerGeneratesEqualResultsOn1_2_3_4_5Array() {
        Queries.TestEq<IQueryable<bool>, bool>(new[] { 1, 2, 3, 4, 5 }, Queries.Qry1);
    }

    [Fact]
    public void ExpressionOptimizerGeneratesSmallerExpressionOn1_2_3_4_5Array() {
        Queries.TestLt(new[] { 1, 2, 3, 4, 5 }, Queries.Qry1);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults1(int[] xs) {
        Queries.TestEq<IQueryable<bool>, bool>(xs, Queries.Qry1);
    }

    [Property]
    public void ExpressionOptimizerGenerateSmallerExpression1(int[] xs) {
        Queries.TestLt(xs, Queries.Qry1);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults2(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<int, int>>, Tuple<int, int>>(xs, Queries.Qry2);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression2(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry2);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults3(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<int, int>>, Tuple<int, int>>(xs, Queries.Qry3);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression3(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry3);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults4(int[] xs) {
        Queries.TestEq<IQueryable<int>, int>(xs, Queries.Qry4);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression4(int[] xs) {
        Queries.TestLt(xs, Queries.Qry4);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults5() {
        Queries.TestEq<IQueryable<Tuple<IGrouping<int, int>, int, int>>, Tuple<IGrouping<int, int>, int, int>>(new[] { 2, -2 }, Queries.Qry5);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression5(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry5);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults6(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<int, int>>, Tuple<int, int>>(xs, Queries.Qry6);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression6(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry6);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults7(int[] xs) {
        Queries.TestEq<IQueryable<bool>, bool>(xs, Queries.Qry7);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression7(int[] xs) {
        Queries.TestLt(xs, Queries.Qry7);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults8(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<int, int>>, Tuple<int, int>>(xs, Queries.Qry8);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression8(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry8);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults9(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<bool, int, string>>, Tuple<bool, int, string>>(xs, Queries.Qry9);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression9(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry9);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults10(int[] xs) {
        Queries.TestEq<IQueryable<int>, int>(xs, Queries.Qry10);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression10(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry10);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults11(int[] xs) {
        Queries.TestEq<IQueryable<int>, int>(xs, Queries.Qry11);
    }

    /// <summary>
    /// This test is broken in the F# version (December 2021).
    /// </summary>
    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression11(int[] xs) {
        // The original F# test uses TestEq...and fails.
        Queries.TestLteq(xs, Queries.Qry11);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults12(int[] xs) {
        Queries.TestEq<IQueryable<int>, int>(xs, Queries.Qry12);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression12(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry12);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults13(int[] xs) {
        Queries.TestEq<IQueryable<bool>, bool>(xs, Queries.Qry13);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression13(int[] xs) {
        Queries.TestLt(xs, Queries.Qry13);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults14(int[] xs) {
        Queries.TestEq<IQueryable<bool>, bool>(xs, Queries.Qry14);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression14(int[] xs) {
        Queries.TestLt(xs, Queries.Qry14);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults15(int[] xs) {
        Queries.TestEq<IQueryable<int>, int>(xs, Queries.Qry15);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression15(int[] xs) {
        Queries.TestLt(xs, Queries.Qry15);
    }

    [Property]
    public void ExpressionOptimizerGeneratesEqualResults16(int[] xs) {
        Queries.TestEq<IQueryable<Tuple<int, int>>, Tuple<int, int>>(xs, Queries.Qry16);
    }

    [Property]
    public void ExpressionOptimizerGeneratesSmallerExpression16(int[] xs) {
        Queries.TestLteq(xs, Queries.Qry16);
    }

    [MemoryDiagnoser()]
    [Serializable]
    public class Benchmark {
        private Expression[] queries;
        internal Func<Expression, IList<object>> VisitAndExecute;

        public Benchmark() {
            var benchmark = this;
            var t = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            this.queries = new Expression[16]
            {
                Queries.Qry1(t).Expression,
                Queries.Qry2(t).Expression,
                Queries.Qry3(t).Expression,
                Queries.Qry4(t).Expression,
                Queries.Qry5(t).Expression,
                Queries.Qry6(t).Expression,
                Queries.Qry7(t).Expression,
                Queries.Qry8(t).Expression,
                Queries.Qry9(t).Expression,
                Queries.Qry10(t).Expression,
                Queries.Qry11(t).Expression,
                Queries.Qry12(t).Expression,
                Queries.Qry13(t).Expression,
                Queries.Qry14(t).Expression,
                Queries.Qry15(t).Expression,
                Queries.Qry16(t).Expression
            };

            VisitAndExecute = expression =>
                Queries.ExecuteExpression<object>(Methods.Visit(expression));
        }

        [GlobalSetup]
        public void Setup() {
        }

        /// <summary>
        /// <para>This test and the ExecuteOpt1 test below take about twice as long to execute as the original F# test on which it is based.
        /// This is because, in F#, a query expression does not work in the way that might be expected.  An IQueryable represents the
        /// Linq expression on which it eas created. In F#, a query expression may return an IQueryable representing a deferred execution
        /// iterator rather than the query expressed in the query expression. This affects half of the queries defined in the F# version of
        /// this code - specifically queries 2, 3, 5, 6, 8, 9, 12 nd 16. When the F# tests are run, these queries provide an expression
        /// for the SelectEnumarableIterator that is used to defer query execution until the results are iterated. Hence, for these tests,
        /// the F# code never gets to work on the underlying Link query. The overhead of these tests for the eight affected queries is
        /// negligible, and hence the F# version of the tests only do approximately half the work that the C# tests do.</para>
        /// 
        /// <para>In the C# version, the code runs over expressions representing the entire query, as written, allowing these eight queries to be
        /// optimised. You cannot perform a direct performance comparison of the C# and F# versions of this code using the tests as written.
        /// If you remove the eight affected queries from the tests and run the benchmark, the performance is roughly equivalent.</para>
        ///
        /// <para>It is possible that the eight affected tests were written without awareness of the underling behaviour of F# query expressions.
        /// The F# tests really need to be rewritten to construct the full query expressions.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [Benchmark(Baseline = true)]
        public void ExecuteDirect() {
            var resultsList = this.queries != null ? new List<object>[this.queries.Length] : throw new ArgumentNullException("queries");
            for (var index = 0; index < resultsList.Length; ++index)
                resultsList[index] = Queries.ExecuteExpression<object>(this.queries[index]);
        }

        [Benchmark()]
        public void ExecuteOpt1() {
            var resultsList = this.queries != null ? new List<object>[this.queries.Length] : throw new ArgumentNullException("queries");
            for (var index = 0; index < resultsList.Length; ++index)
                resultsList[index] = VisitAndExecute(this.queries[index]).ToList();
        }
    }
}