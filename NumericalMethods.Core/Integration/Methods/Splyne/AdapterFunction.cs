using NumericalMethods.Core.Integration.Interfaces;

namespace NumericalMethods.Core.Integration.Methods.Splyne
{
    public record class AdapterFunction(IIntegrand Function) : IFunction
    {        
        public double Calculate(double x)
        {
            return Function.Calculate(x);
        }
    }
}
