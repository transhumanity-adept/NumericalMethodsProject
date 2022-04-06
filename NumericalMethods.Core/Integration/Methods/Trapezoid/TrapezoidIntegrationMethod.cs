using NumericalMethods.Core.Integration.Interfaces;

namespace NumericalMethods.Core.Integration.Methods.Trapezoid;
public class TrapezoidIntegrationMethod : IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, double start, double end, double step)
    {
        double sumElement = 0;
        for (double x = start + step; x < end; x += step)
        {
            sumElement += step * (function.Calculate(x - step) + function.Calculate(x));
        }
        return sumElement / 2;
    }
}