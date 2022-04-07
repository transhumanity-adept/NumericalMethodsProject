using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.Core.Integration.Methods.Rectangle;
using NumericalMethods.Core.Integration.Methods.Parabolic;
using NumericalMethods.Core.Integration.Methods.Trapezoid;

namespace NumericalMethods.Core.Integration;
public static class IntegrationBuilder
{
    public static IIntegratorWithConstantStep Build(IIntegrand function, IntegrationMethodsWithConstantStep method)
    {
        return method switch
        {
            IntegrationMethodsWithConstantStep.Rectangle => new IntegratorWithConstantStep(new RectangleIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Trapeze => new IntegratorWithConstantStep(new TrapezoidIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Parabolic => new IntegratorWithConstantStep(new ParabolicIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Spline => throw new NotImplementedException()
        };
    }

    public static IIntegratorWithVariableStep Build(IIntegrand function, IntegrationMethodsWithVariableStep method)
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
