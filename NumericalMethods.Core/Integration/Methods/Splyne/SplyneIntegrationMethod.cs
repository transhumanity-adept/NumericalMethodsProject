using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.Core.Integration.Methods.Trapezoid;

namespace NumericalMethods.Core.Integration.Methods.Splyne
{
    public record class SplyneIntegrationMethod : IIntegrationMethodWithConstantStep
    {
        TrapezoidIntegrationMethod trapezoid = new TrapezoidIntegrationMethod();
        
        public double Intergrate(IIntegrand function, double start, double end, double step)
        {
            double result = 0;
            double trapezoidIntegral = trapezoid.Intergrate(function, start, end, step); // Первая часть функции Сплайна
            for (double x = start + step; x < end; x += step)
            {
                result += Math.Pow(step, 3); // Второе слагаемое - производная
            }
            return trapezoidIntegral - result/12;
        }
    }
}
