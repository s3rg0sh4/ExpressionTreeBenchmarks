namespace test.ExpressionTests;
using System;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class CtorTest
{
	private static Func<Dull> _ctor;

	public static void RunBenchmark() => BenchmarkRunner.Run<CtorTest>();

	[Benchmark(Baseline = true)]
	public void DefaultCtorTest() => _ = new Dull();

	[Benchmark]
	public void ActivatorTest() => _ = (Dull)Activator.CreateInstance(typeof(Dull))!;

	[Benchmark]
	public void ExpressionTest() => _ = (_ctor ??= CreateCtor())();

	private static Func<Dull> CreateCtor()
	{
		return Expression.Lambda<Func<Dull>>(Expression.New(typeof(Dull).GetConstructor([]))).Compile();
	}
}
