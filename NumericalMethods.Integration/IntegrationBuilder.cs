using NumericalMethods.Integration.Interfaces;
using NumericalMethods.Integration.Methods;
using NumericalMethods.Integration.Methods.Rectangle;

namespace NumericalMethods.Integration;
public static class IntegrationBuilder
{
    public static IIntegratorWithConstantStep Build(IIntegrand function, IntegrationMethodsWithConstantStep method)
    {
        return method switch
        {
            IntegrationMethodsWithConstantStep.Rectangle => new IntegratorWithConstantStep(new RectangleIntegrationMethod(), function),
            IntegrationMethodsWithConstantStep.Trapeze => throw new NotImplementedException(),
            IntegrationMethodsWithConstantStep.Parabolic => throw new NotImplementedException(),
            IntegrationMethodsWithConstantStep.Spline => throw new NotImplementedException(),
            IntegrationMethodsWithConstantStep.MonteCarlo => throw new NotImplementedException(),
            IntegrationMethodsWithConstantStep.Gauss => throw new NotImplementedException(),
            IntegrationMethodsWithConstantStep.Chebyshev => throw new NotImplementedException()
        };
    }

    public static IIntegratorWithVariableStep Build(IIntegrand function, IntegrationMethodsWithVariableStep method)
    {
        var test = function;
        return method switch
        {
            IntegrationMethodsWithVariableStep.Gauss => throw new NotImplementedException(),
            IntegrationMethodsWithVariableStep.Chebyshev => throw new NotImplementedException()
        };
    }
}
