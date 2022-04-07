using NumericalMethods.Core.Integration.Interfaces;

namespace NumericalMethods.Core.Integration.Methods;
internal interface IIntegrationMethodWithVariableStep
{
    public double Intergrate(IIntegrand function, double start, double end, int count_nodes);
}