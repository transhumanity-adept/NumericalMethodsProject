using NumericalMethods.Integration.Interfaces;

namespace NumericalMethods.Integration.Methods.Rectangle;
internal class RectangleIntegrationMethod : IIntegrationMethodWithConstantStep
{
    public double Intergrate(IIntegrand function, IntegrationIntervalWithStep interval)
    {
        double resulted_sum = 0;
        for (double xi = interval.Start + (interval.Step / 2); xi < interval.End; xi += interval.Step)
        {
            resulted_sum += (interval.Step * function.Calculate(xi));
        }

        return resulted_sum;
    }
}