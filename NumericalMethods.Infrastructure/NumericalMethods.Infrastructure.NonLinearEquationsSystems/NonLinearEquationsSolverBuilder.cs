using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Interfaces;
using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods.Newton;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems;
public record class NonLinearEquationsSolverBuilder()
{
	public INonLinearEquationsSystemsSolver Build(SolvingMethods SolvingMethod)
	{
		return SolvingMethod switch
		{
			SolvingMethods.Newton => new NonLinearEquationsSystemsSolver(new NewtonMethod()),
			_ => throw new NotImplementedException()
		};
	}
}
