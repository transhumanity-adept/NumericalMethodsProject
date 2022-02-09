namespace NumericalMethods.Differentiations.Interfaces;
public interface IDifferentiationFunction
{
    public double? Calculate(double argument, int derivative_degree);
}
