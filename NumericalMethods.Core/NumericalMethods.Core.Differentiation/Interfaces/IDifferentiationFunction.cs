namespace NumericalMethods.Core.Differentiations.Interfaces;
public interface IDifferentiationFunction
{
	public double? Calculate(double argument, int derivative_degree);
}
