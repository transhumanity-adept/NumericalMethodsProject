using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.Core.Integration.Methods;

namespace NumericalMethods.Core.Integration;
internal record class IntegratorWithVariableStep(IIntegrationMethodWithVariableStep IntegrationMethod, IIntegrand Function) : IIntegratorWithVariableStep
{
    public double Integrate(double start, double end, int count_nodes)
    {
        return IntegrationMethod.Intergrate(Function, start, end, count_nodes);
    }
}
