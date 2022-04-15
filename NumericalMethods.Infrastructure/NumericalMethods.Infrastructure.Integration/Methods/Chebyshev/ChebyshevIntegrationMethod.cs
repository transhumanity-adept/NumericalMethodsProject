using NumericalMethods.Infrastructure.Integration.Interfaces;

namespace NumericalMethods.Infrastructure.Integration.Methods.Chebyshev;
public class ChebyshevIntegrationMethod : IIntegrationMethodWithVariableStep
{
    public double Intergrate(IIntegrand function, double start, double end, int count_nodes)
    {
        throw new NotImplementedException();
    }
}
