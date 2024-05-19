namespace test.ExpressionTests;
using System;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class GetterTest
{
	private Dull dull = new();

	private static Func<Dull, object> getter;

	public static void RunBenchmark() => BenchmarkRunner.Run<GetterTest>();

	[Benchmark(Baseline = true)]
	public void DefaultGetterTest()	=> _ = dull.A;

	[Benchmark]
	public void GetVauleTest() => _ = typeof(Dull).GetProperty(nameof(Dull.A)).GetValue(dull);

	[Benchmark]
	public void ExpressionTest() => _ = (getter ??= CompileGetter())(dull);

	private static Func<Dull, object> CompileGetter()
	{
		var propertyInfo = typeof(Dull).GetProperty(nameof(Dull.A));

		var instanceBase = Expression.Variable(typeof(Dull), "instance");
		var instance = Expression.Convert(instanceBase, propertyInfo.DeclaringType);

		var method = propertyInfo.GetGetMethod();
		var call = Expression.Call(instance, method!);

		return Expression.Lambda<Func<Dull, object>>(call, instanceBase).Compile();
	}
}