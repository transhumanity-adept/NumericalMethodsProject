using NumericalMethods.Integration.Interfaces;

namespace NumericalMethods.Integration.Methods;
internal interface IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, double start, double end, double step);
}
