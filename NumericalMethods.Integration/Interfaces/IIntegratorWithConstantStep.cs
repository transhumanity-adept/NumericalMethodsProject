using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration.Interfaces;
public interface IIntegratorWithConstantStep
{
    public double Integrate(double start, double end, double step);
}