using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration.Interfaces;
public interface IIntegratorWithConstantStep
{
    public double Integrate(IntegrationIntervalWithStep interval);
}