using NumericalMethods.Integration.Interfaces;
using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration;
internal record class IntegratorWithConstantStep(IIntegrationMethodWithConstantStep IntegrationMethod, IIntegrand Function) : IIntegratorWithConstantStep
{
    public double Integrate(double start, double end, double step)
    {
        return IntegrationMethod.Intergrate(Function, start, end, step);
    }
}