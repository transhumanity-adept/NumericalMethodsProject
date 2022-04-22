using NumericalMethods.Infrastructure.Integration.Interfaces;
using NumericalMethods.Infrastructure.Integration.Methods.Parabolic;
using NumericalMethods.Infrastructure.Integration.Methods.Rectangle;
using NumericalMethods.Infrastructure.Integration.Methods.Splyne;
using NumericalMethods.Infrastructure.Integration.Methods.Trapezoid;

namespace NumericalMethods.Infrastructure.Integration;
public record class IntegrationBuilder
{
	public IIntegratorWithConstantStep Build(IIntegrand function, IntegrationMethodsWithConstantStep method)
	{
		return method switch
		{
			IntegrationMethodsWithConstantStep.Rectangle => new IntegratorWithConstantStep(new RectangleIntegrationMethod(), function),
			IntegrationMethodsWithConstantStep.Trapeze => new IntegratorWithConstantStep(new TrapezoidIntegrationMethod(), function),
			IntegrationMethodsWithConstantStep.Parabolic => new IntegratorWithConstantStep(new ParabolicIntegrationMethod(), function),
			IntegrationMethodsWithConstantStep.Spline => new IntegratorWithConstantStep(new SplyneIntegrationMethod(), function),
			_ => new IntegratorWithConstantStep(new SplyneIntegrationMethod(), function)
		};
	}

	public IIntegratorWithVariableStep Build(IIntegrand function, IntegrationMethodsWithVariableStep method)
	{
		var test = function;
		return method switch
		{
			IntegrationMethodsWithVariableStep.Gauss => throw new NotImplementedException(),
			IntegrationMethodsWithVariableStep.Chebyshev => throw new NotImplementedException(),
			IntegrationMethodsWithVariableStep.MonteCarlo => throw new NotImplementedException(),
			_ => throw new NotImplementedException()
		};
	}
}
