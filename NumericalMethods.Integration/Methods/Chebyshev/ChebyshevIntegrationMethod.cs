using NumericalMethods.Integration.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Integration.Methods.Chebyshev;
public class ChebyshevIntegrationMethod : IIntegrationMethodWithVariableStep
{
    public double Intergrate(IIntegrand function, params IntegrationInterval[] intervals)
    {
        throw new NotImplementedException();
    }
}
