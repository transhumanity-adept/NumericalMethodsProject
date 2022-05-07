using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.Linear;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.Quadratic;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.NewtonPolynomial;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations;
public static class DifferentiationBuilder
{
	public static IDifferentiationFunction? Build(IEnumerable<IDifferentiationNode> differentiationNodes, DifferentiationFunctionType functionType, double step)
	{
		return functionType switch
		{
			DifferentiationFunctionType.Linear => new LinearDifferentiationFunction(differentiationNodes, step),
			DifferentiationFunctionType.Quadratic => new QuadraticDifferentationFunction(differentiationNodes, step),
			_ => null,
		};
	}

	public static INewtonDifferentiationFunction CreateNewton(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derivative_degree, int numberOfMembers)
	{
		return new NewtonPolynomialDifferentiationFunction(differentiationNodes, step, derivative_degree, numberOfMembers);
	}
}