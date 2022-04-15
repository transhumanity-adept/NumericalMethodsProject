using NumericalMethods.Infrastructure.Integration.Interfaces;

using org.mariuszgromada.math.mxparser;

namespace NumericalMethods.WPFApplication.Integration;
public class Integrand : IIntegrand
{
    private readonly Function _fucntion;
    public Integrand(Function function)
    {
        _fucntion = function;
    }
    public double Calculate(double argument)
    {
        return _fucntion.calculate(argument);
    }
}
