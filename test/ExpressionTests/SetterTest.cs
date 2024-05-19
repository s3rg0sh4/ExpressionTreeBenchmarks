namespace test.ExpressionTests;
using System;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class SetterTest
{
	private Dull dull = new();

	private static Action<Dull, object> setter;

	public static void RunBenchmark() => BenchmarkRunner.Run<SetterTest>();

	[Benchmark(Baseline = true)]
	public void DefaultSetterTest() => dull.A = new();

	[Benchmark]
	public void SetVauleTest() => typeof(Dull).GetProperty(nameof(Dull.A)).SetValue(dull, new());

	[Benchmark]
	public void ExpressionTest() => (setter ??= CompileSetter())(dull, new());

	private static Action<Dull, object> CompileSetter()
	{
		var propertyInfo = typeof(Dull).GetProperty(nameof(Dull.A));

		var instanceBase = Expression.Variable(typeof(Dull), "instance");
		var instance = Expression.Convert(instanceBase, propertyInfo.DeclaringType);

		var value = Expression.Variable(typeof(object), "value");

		var method = propertyInfo.GetSetMethod();
		var call = Expression.Call(instance, method!, value);

		return Expression.Lambda<Action<Dull, object>>(call, instanceBase, value).Compile();
	}
}