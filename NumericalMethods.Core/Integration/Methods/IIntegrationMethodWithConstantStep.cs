using NumericalMethods.Core.Integration.Interfaces;

namespace NumericalMethods.Core.Integration.Methods;
internal interface IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, double start, double end, double step);
}
