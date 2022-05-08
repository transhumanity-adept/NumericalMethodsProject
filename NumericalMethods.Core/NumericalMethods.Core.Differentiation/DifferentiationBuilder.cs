using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.Linear;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.Quadratic;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.NewtonPolynomial;
using NumericalMethods.Core.Differentiations.DifferentiationFunctions.UndefinedCoefficients;
using NumericalMethods.Core.Differentiations.Interfaces;
using NumericalMethods.Core.Differentiation.DifferentiationFunctions.Cubic;

namespace NumericalMethods.Core.Differentiations;
public static class DifferentiationBuilder
{
	public static IDifferentiationFunction? Build(IEnumerable<IDifferentiationNode> differentiation_nodes, DifferentiationFunctionType function_type, double step, int derrivative_degree)
	{
		return function_type switch
		{
			DifferentiationFunctionType.Linear => new LinearDifferentiationFunction(differentiation_nodes, step, derrivative_degree),
			DifferentiationFunctionType.Quadratic => new QuadraticDifferentationFunction(differentiation_nodes, step, derrivative_degree),
			DifferentiationFunctionType.Cubic => new CubicDifferentiationFunction(differentiation_nodes, step, derrivative_degree),
			_ => throw new NotImplementedException()
		};
	}

	public static INewtonDifferentiationFunction CreateNewton(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derivative_degree, int numberOfMembers)
	{
		return new NewtonPolynomialDifferentiationFunction(differentiationNodes, step, derivative_degree, numberOfMembers);
	}
	public static IUndefinedCoefficientsDifferentiationFunction CreateUndefinedCoefficients(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degre)
    {
		return new UndefinedCoefficientsDifferentiationFunction(differentiationNodes, step, derrivative_degre);
    }
}