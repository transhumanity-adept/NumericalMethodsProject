namespace NumericalMethods.Core.Integration.Interfaces;
public interface IIntegratorWithConstantStep
{
    public double Integrate(double start, double end, double step);
}