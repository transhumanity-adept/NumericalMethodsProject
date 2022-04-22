using NumericalMethods.Core.Differentiations.DifferentiationFunctions.Linear;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations;
public static class DifferentiationBuilder
{
	public static IDifferentiationFunction? Build(IEnumerable<IDifferentiationNode> interpolation_nodes, DifferentiationFunctionType function_type, double step)
	{
		return function_type switch
		{
			DifferentiationFunctionType.Linear => new LinearDifferentiationFunction(interpolation_nodes, step),
			_ => null
		};
	}
}