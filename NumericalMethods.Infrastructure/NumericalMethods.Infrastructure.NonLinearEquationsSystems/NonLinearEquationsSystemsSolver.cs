using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Interfaces;
using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems;
public record class NonLinearEquationsSystemsSolver(ISolvingMethod SolvingMethod) : INonLinearEquationsSystemsSolver
{
	public IEnumerable<double> Solve(NonLinearEquationsSystem system, double eps)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<IEnumerable<double>> SolveWithSteps(NonLinearEquationsSystem system, double eps)
	{
		throw new NotImplementedException();
	}
}
