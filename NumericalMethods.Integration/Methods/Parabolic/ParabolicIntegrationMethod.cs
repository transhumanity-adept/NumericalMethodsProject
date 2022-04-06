using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalMethods.Integration.Interfaces;

namespace NumericalMethods.Integration.Methods.Parabolic
{
    public class ParabolicIntegrationMethod : IIntegrationMethodWithConstantStep
    {
        public double Intergrate(IIntegrand function, double start, double end, double step)
        {
            double valueEvenFunction = 0, valueOddFunction = 0;
            int i = 0;
            for (double x = start+step; x < end-step; x+=step)
                if (++i % 2 == 0)
                    valueEvenFunction += function.Calculate(x);
                else
                    valueOddFunction += function.Calculate(x);
            return step / 3 * (function.Calculate(start) + 4 * valueOddFunction + 2 * valueEvenFunction + function.Calculate(end));
        }
    }
}
