using NumericalMethods.Integration.Methods;

namespace NumericalMethods.Integration.Interfaces;
public interface IIntegratorWithVariableStep
{
    public double Integrate(params IntegrationIntervalWithStep[] intervals);
}
