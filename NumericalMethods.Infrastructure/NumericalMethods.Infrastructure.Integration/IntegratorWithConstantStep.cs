using NumericalMethods.Infrastructure.Integration;
using NumericalMethods.Infrastructure.Integration.Interfaces;
using NumericalMethods.Infrastructure.Integration.Methods;

namespace NumericalMethods.Infrastructure.Integration;
internal record class IntegratorWithConstantStep(IIntegrationMethodWithConstantStep IntegrationMethod, IIntegrand Function) : IIntegratorWithConstantStep
{
    public double Integrate(double start, double end, double step)
    {
        return IntegrationMethod.Intergrate(Function, start, end, step);
    }
}