using NumericalMethods.Infrastructure.Integration.Interfaces;

namespace NumericalMethods.Infrastructure.Integration.Methods;
internal interface IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, double start, double end, double step);
}
