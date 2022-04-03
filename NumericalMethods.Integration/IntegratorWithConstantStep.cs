using NumericalMethods.Integration.Interfaces;
using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration;
internal record class IntegratorWithConstantStep(IIntegrationMethodWithConstantStep IntegrationMethod, IIntegrand Function) : IIntegratorWithConstantStep
{
    public double Integrate(IntegrationIntervalWithStep interval)
    {
        return IntegrationMethod.Intergrate(Function, interval);
    }
}