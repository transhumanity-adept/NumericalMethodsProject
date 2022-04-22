namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Interfaces;
public interface IFunction
{
	public double Calculate(IEnumerable<double> parameters);
}
