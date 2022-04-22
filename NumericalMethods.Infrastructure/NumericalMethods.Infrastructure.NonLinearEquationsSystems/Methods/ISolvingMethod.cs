namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods;
public interface ISolvingMethod
{
	public IEnumerable<double> Solve(NonLinearEquationsSystem system, double eps);
	public IEnumerable<IEnumerable<double>> SolveWithSteps(NonLinearEquationsSystem system, double eps);
}
