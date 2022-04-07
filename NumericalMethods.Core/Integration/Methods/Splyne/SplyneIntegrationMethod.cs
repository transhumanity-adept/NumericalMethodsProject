using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalMethods.Integration.Interfaces;

namespace NumericalMethods.Integration.Methods.Splyne
{
    public class SplyneIntegrationMethod : IIntegrationMethodWithConstantStep
    {
        public double Intergrate(IIntegrand function, double start, double end, double step)
        {
            throw new NotImplementedException();
        }
    }
}
