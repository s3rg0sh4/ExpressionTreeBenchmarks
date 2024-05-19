using test.ExpressionTests;

CtorTest.RunBenchmark();
GetterTest.RunBenchmark();
SetterTest.RunBenchmark();

/// <summary>
/// Класс-заглушка
/// </summary>
public class Dull
{
	public object A { get; set; }
}