using NumericalMethods.Core.Integration;
using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.Core.Integration.Methods;

namespace NumericalMethods.Core.Integration;
internal record class IntegratorWithConstantStep(IIntegrationMethodWithConstantStep IntegrationMethod, IIntegrand Function) : IIntegratorWithConstantStep
{
    public double Integrate(double start, double end, double step)
    {
        return IntegrationMethod.Intergrate(Function, start, end, step);
    }
}