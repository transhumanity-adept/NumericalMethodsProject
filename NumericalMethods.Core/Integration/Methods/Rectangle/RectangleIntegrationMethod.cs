using NumericalMethods.Core.Integration.Interfaces;

namespace NumericalMethods.Core.Integration.Methods.Rectangle;
internal class RectangleIntegrationMethod : IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, double start, double end, double step)
    {
        double resulted_sum = 0;
        for (double xi = start + (step / 2); xi < end; xi += step)
        {
            resulted_sum += (step * function.Calculate(xi));
        }

        return resulted_sum;
    }
}