using NumericalMethods.Infrastructure.Integration.Interfaces;

namespace NumericalMethods.Infrastructure.Integration.Methods;
internal interface IIntegrationMethodWithVariableStep
{
	public double Intergrate(IIntegrand function, double start, double end, int count_nodes);
}