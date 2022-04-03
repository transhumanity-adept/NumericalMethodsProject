using NumericalMethods.Integration.Interfaces;
using NumericalMethods.Integration.Methods;
using NumericalMethods.Integration.Methods.Rectangle;

namespace NumericalMethods.Integration;
public static class IntegrationBuilder
{
    public static IIntegratorWithConstantStep Build(IIntegrand function, IntergrationMethodsWithConstantStep method)
    {
        return method switch
        {
            IntergrationMethodsWithConstantStep.Rectangle => new IntegratorWithConstantStep(new RectangleIntegrationMethod(), function),
            IntergrationMethodsWithConstantStep.Trapeze => throw new NotImplementedException(),
            IntergrationMethodsWithConstantStep.Parabolic => throw new NotImplementedException(),
            IntergrationMethodsWithConstantStep.Spline => throw new NotImplementedException(),
            IntergrationMethodsWithConstantStep.MonteCarlo => throw new NotImplementedException(),
            IntergrationMethodsWithConstantStep.Gauss => throw new NotImplementedException(),
            IntergrationMethodsWithConstantStep.Chebyshev => throw new NotImplementedException()
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
