using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods.Newton;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems;
public record class NonLinearEquationsSolverBuilder()
{
	public static NonLinearEquationsSystemsSolver Build(NonLinearEquationsSolvingMethods SolvingMethod)
	{
		return SolvingMethod switch
		{
			NonLinearEquationsSolvingMethods.Newton => new NonLinearEquationsSystemsSolver(new NewtonMethod()),
			_ => throw new NotImplementedException()
		};
	}
}
