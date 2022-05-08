using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Core.Differentiation.DifferentiationFunctions.UndefinedCoefficients
{
    internal struct LinerEquation
    {
        public IEnumerable<double> Coefficients { get; private set; }
        public double FreeMember { get; private set; }
        public LinerEquation(IEnumerable<double> coefficients, double freeMember)
        {
            Coefficients = coefficients;
            FreeMember = freeMember;
        }
    }
}
