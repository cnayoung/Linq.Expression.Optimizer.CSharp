using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Methods;
using FsUnit;

namespace Tests {
    record Itm(int x);

    // The original F# code for these queries has been rewritten here using C# query syntax wherever possible.
    internal static class Queries {
        public static List<T> ExecuteExpression<T>(Expression e) =>
            ((IEnumerable)Expression.Lambda(e).Compile().DynamicInvoke()).Cast<T>().ToList();

        public static (IList<a>, IList<a>) TestExpression<b, a>(IQueryable<b> qry) {

            var optimized = ExpressionOptimizer.Visit(qry.Expression);
            var expected = ExecuteExpression<a>(qry.Expression);
            return optimized.GetHashCode() != qry.Expression.GetHashCode()
                ? (expected, ExecuteExpression<a>(optimized))
                : (expected, expected);
        }

        public static IQueryable<bool> Qry1(IList<int> arr) =>
            from x in arr.AsQueryable()
            select !(!(!(x > 3))) && true;

        /// <summary>
        /// There is no way to 'cheat' the C# compiler using query syntax to prevent it from optimizing
        /// out the redundant condition expression (where (true && true) || (true && true)). We have to
        /// construct the expression directly to test the ability of the expression optimizer code to
        /// handle this redundancy.
        /// </summary>
        public static IQueryable<Tuple<int, int>> Qry2(IList<int> arr) {
            var parameterExpression = Expression.Parameter(typeof(int), "x");
            var q1 = arr.AsQueryable().Where(Expression.Lambda<Func<int, bool>>(
                Expression.OrElse(
                    Expression.AndAlso(
                        Expression.Constant(true, typeof(bool)),
                        Expression.Constant(true, typeof(bool))),
                    Expression.AndAlso(
                        Expression.Constant(true, typeof(bool)),
                        Expression.Constant(true, typeof(bool)))),
                parameterExpression)).Select(Expression.Lambda<Func<int, int>>(
                Expression.Add(parameterExpression, Expression.Constant(3, typeof(int))), parameterExpression));

            return from x in q1
                   let x2 = (x - 1)
                   let x3 = (x + 1)
                   select Tuple.Create(x2, x3);
        }

        public static IQueryable<Tuple<int, int>> Qry3(IList<int> arr) =>
            from x in arr.AsQueryable()
            group x by x into g
            select Tuple.Create(g.Key, g.Count());

        /// <summary>
        /// There is no way to 'cheat' the C# compiler using query syntax to prevent it from optimizing
        /// out the redundant condition expression (true ? x | x). We have to construct the expression
        /// directly to test the ability of the expression optimizer code to handle this redundancy.
        /// </summary>
        public static IQueryable<int> Qry4(IList<int> arr) {
            var paramExpr = Expression.Parameter(typeof(int), "x");
            var expr = (MethodCallExpression)arr.AsQueryable().OrderByDescending(x => x).ThenBy(x => x)
                .Select(x => x > 0 ? x : 1).Expression;  // Create a dummy condition expression which we will then replace.
            return new EnumerableQuery<int>(Expression.Call(expr.Object, expr.Method, expr.Arguments[0],
                Expression.Quote(Expression.Lambda<Func<int, int>>(
                    Expression.Condition(Expression.Constant(true), paramExpr, paramExpr), paramExpr))));
        }

        public static IQueryable<Tuple<IGrouping<int, int>, int, int>> Qry5(IList<int> arr) =>
            from x in arr.AsQueryable()
            group x by x
            into g
            select Tuple.Create(g, g.Key, g.Count());

        public static IQueryable<Tuple<int, int>> Qry6(IList<int> arr) =>
            from x1 in arr.AsQueryable()
            join x2 in arr.AsQueryable()
                on x1 equals x2
            select Tuple.Create(x1, x2);

        public static IQueryable<bool> Qry7(IList<int> arr) =>
            from x in arr.AsQueryable()
            let tmp = x
            select !(!(!(x > 3))) && true;

        public static IQueryable<Tuple<int, int>> Qry8(IList<int> arr) =>
          from x1 in arr.AsQueryable()
          join x2 in arr.AsQueryable()
          on x1 + 1 equals x2 into g
          join x3 in arr.AsQueryable()
          on x1 equals x3
          select Tuple.Create(x1, x3);

        public static IQueryable<Tuple<bool, int, string>> Qry9(IList<int> arr) =>
          from x1 in arr.AsQueryable()
          join x2 in arr.AsQueryable()
          on x1 equals x2 into r
          from y in r.DefaultIfEmpty()
          select Tuple.Create(true, y, "asdf");

        public static IQueryable<int> Qry10(IList<int> arr) =>
          from x in arr.AsQueryable()
          where (from y in arr.AsQueryable()
                 where x == -y
                 select y).Any()
          select x;

        public static int cond1 = 12;

        public static IQueryable<int> Qry11(IList<int> arr) =>
         (from x in arr.AsQueryable()
          select x)
            .Distinct()
            .SkipWhile(_ => cond1 < 3)
            .Skip(1)
            .Take(4)
            .OrderBy(x => (int?)x);
        private static int f(int v) => v;

        public static IQueryable<int> Qry12(IList<int> arr) =>
            from x in arr.AsQueryable()
            where cond1 == 12
            where cond1 == 12 && true && f(x) > -1
            let c = 2
            let y = cond1
            let y2 = x
            where cond1 == 12
            let tmp = y + c + f(x)
            where tmp > 1
            orderby (int?)x
            select x;

        public static IQueryable<bool> Qry13(IList<int> arr) =>
            from x in arr.AsQueryable()
            where Tuple.Create(x, true) == Tuple.Create(x, true) && true
            select (x > 0 && x > 1 && x > 2 && x > 3 && x > 4 && x > 0 && x > 1 && x > 2 && x > 3 && x > 4) ||
            x < 0 || x < 1 || x < 2 || x < 3 || x < 4 || x < 0 || x < 1 || x < 2 || x < 3 || x < 4;

        public static IQueryable<bool> Qry14(IList<int> arr) {
            var aq = arr.AsQueryable();

            return from x in arr.AsQueryable()
                   where arr == arr
                   where aq == aq.Reverse().Reverse().AsQueryable()
                   where arr.Equals(new[] { 1, 2, 3 })
                   select !(!(!(x > 3))) && true;
        }

        public static IQueryable<int> Qry15(IList<int> arr) {
            int? y = null;
            object? xx = new int?();

            return from x in arr.AsQueryable()
                   where !(((int?)xx).HasValue || (int?)xx > 2) && (!y.HasValue || y.Value > x) && true
                   select 1;
        }

        private static Itm ToImts(int x) => new(x);

        public static IQueryable<Tuple<int, int>> Qry16(IList<int> arr) {
            var asItms = arr.Select(ToImts);
            var arr2 = asItms.AsQueryable();
            var onlyX = true;

            return from i in arr2
                   join j in arr2
                       on i.x equals j.x
                   where ((!onlyX) || (onlyX && i.x == 3)) && arr2.Any(sl => false || sl.x == j.x && j.x != 1)
                   select Tuple.Create(i.x, j.x);
        }

        public static void TestEq<a, b>(int[] xs, Func<IList<int>, a> qry) where a : IQueryable<b> {
            var res = TestExpression<b, object?>(qry(xs.ToList()));
            Assert.AreEqual(res.Item1, res.Item2);
        }
        public static void TestLt<a>(int[] xs, Func<IList<int>, IQueryable<a>> qry) {
            var expr = qry(xs.ToList()).Expression;
            var optimized = ExpressionOptimizer.Visit(expr);
            var o = optimized.ToString();
            var o2 = o.ToString();
            Assert.Less(optimized.ToString().Length, expr.ToString().Length);
        }

        public static void TestLteq<a>(int[] xs, Func<IList<int>, IQueryable<a>> qry) {
            var expr = qry(xs.ToList()).Expression;
            var optimized = ExpressionOptimizer.Visit(expr);
            Assert.LessOrEqual(optimized.ToString().Length, expr.ToString().Length);
        }
    }
}