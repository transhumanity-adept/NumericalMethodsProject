using NumericalMethods.Integration.Interfaces;
using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration;
internal record class IntegratorWithVariableStep(IIntegrationMethodWithVariableStep IntegrationMethod, IIntegrand Function) : IIntegratorWithVariableStep
{
    public double Integrate(params IntegrationInterval[] intervals)
    {
        return IntegrationMethod.Intergrate(Function, intervals);
    }
}
