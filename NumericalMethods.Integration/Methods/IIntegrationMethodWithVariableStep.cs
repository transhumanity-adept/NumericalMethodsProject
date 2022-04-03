using NumericalMethods.Integration.Interfaces;

namespace NumericalMethods.Integration.Methods;
internal interface IIntegrationMethodWithVariableStep
{
    public double Intergrate(IIntegrand function, params IntegrationIntervalWithStep[] intervals);
}