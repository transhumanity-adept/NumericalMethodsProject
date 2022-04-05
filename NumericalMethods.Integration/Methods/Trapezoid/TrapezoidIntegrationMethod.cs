using NumericalMethods.Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Integration.Methods.Trapezoid
{
    public class TrapezoidIntegrationMethod : IIntegrationMethodWithConstantStep
    {
        public double Intergrate(IIntegrand function, double start, double end, double step)
        {
            double sumElement = 0;
            for (double x = start+step; x < end; x+=step)
            {
                sumElement += step * (function.Calculate(x-step) + function.Calculate(x));
            }
            return sumElement / 2;
        }
    }
}
