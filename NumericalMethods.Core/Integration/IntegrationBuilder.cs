using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.Core.Integration.Methods.Rectangle;
using NumericalMethods.Core.Integration.Methods.Parabolic;
using NumericalMethods.Core.Integration.Methods.Trapezoid;
using NumericalMethods.Core.Integration.Methods.Splyne;

namespace NumericalMethods.Core.Integration;
public record class IntegrationBuilder
{
    public IIntegratorWithConstantStep Build(IIntegrand function, IntegrationMethodsWithConstantStep method)
    {
        return method switch
        {
            IntegrationMethodsWithConstantStep.Rectangle => new IntegratorWithConstantStep(new RectangleIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Trapeze => new IntegratorWithConstantStep(new TrapezoidIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Parabolic => new IntegratorWithConstantStep(new ParabolicIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Spline =>  new IntegratorWithConstantStep(new SplyneIntegrationMethod(), function)
        };
    }

    public IIntegratorWithVariableStep Build(IIntegrand function, IntegrationMethodsWithVariableStep method)
    {
        var test = function;
        return method switch
        {
            IntegrationMethodsWithVariableStep.Gauss => throw new NotImplementedException(),
            IntegrationMethodsWithVariableStep.Chebyshev => throw new NotImplementedException(),
            IntegrationMethodsWithVariableStep.MonteCarlo => throw new NotImplementedException()
        };
    }
}
