using NumericalMethods.Approximation.Interpolations.Interfaces;
using NumericalMethods.Approximation.Interpolations.InterpolationFuncitons.Cubic;
using NumericalMethods.Approximation.Interpolations.InterpolationFuncitons.Linear;

namespace NumericalMethods.Approximation.Interpolations;
public static class InterpolationBuilder
{
    public static IInterpolationFunction? Build(IEnumerable<IInterpolationNode> interpolation_nodes, InterpolationFunctionType function_type)
    {
        return function_type switch
        {
            InterpolationFunctionType.Linear => new LinearInterpolationFunction(interpolation_nodes),
            InterpolationFunctionType.Cubic => new СubicInterpolationFunction(interpolation_nodes),
            _ => null
        };
    }
}
