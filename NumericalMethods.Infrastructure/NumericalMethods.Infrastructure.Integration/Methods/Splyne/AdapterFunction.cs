using NumericalMethods.Infrastructure.Integration.Interfaces;

namespace NumericalMethods.Infrastructure.Integration.Methods.Splyne
{
	public record class AdapterFunction(IIntegrand Function) : IFunction
	{
		public double Calculate(double x)
		{
			return Function.Calculate(x);
		}
	}
}
