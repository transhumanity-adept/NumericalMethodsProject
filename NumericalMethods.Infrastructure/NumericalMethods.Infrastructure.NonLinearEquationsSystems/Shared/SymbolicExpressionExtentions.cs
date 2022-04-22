using MathNet.Symbolics;

using System.Runtime.CompilerServices;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems;
public static class SymbolicExpressionExtentions
{
	public static SymbolicExpression GetFreeMember(this SymbolicExpression symbolicExpression)
	{
		IEnumerable<SymbolicExpression>? products = symbolicExpression.CollectProducts();
		SymbolicExpression result = symbolicExpression;
		foreach (SymbolicExpression? product in products)
		{
			result = result.Subtract(product);
		}

		return result;
	}

	public static double EvaluateOfList(this SymbolicExpression function, List<(string argumentName, double argumentValue)> values)
	{
		Dictionary<string, FloatingPoint>? valuesDictionary = new Dictionary<string, FloatingPoint>();
		foreach ((string argumentName, double argumentValue) in values)
		{
			valuesDictionary.Add(argumentName, argumentValue);
		}

		return function.Evaluate(valuesDictionary).RealValue;
	}
}
