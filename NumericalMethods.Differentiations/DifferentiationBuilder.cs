using NumericalMethods.Differentiations.DifferentiationFunctions.Linear;
using NumericalMethods.Differentiations.Interfaces;

namespace NumericalMethods.Differentiations;
public static class DifferentiationBuilder
{
    public static IDifferentiationFunction? Build(IEnumerable<IInterpolationNode> interpolation_nodes, DifferentiationFunctionType function_type, double step)
    {
        return function_type switch
        {
            DifferentiationFunctionType.Linear => new LinearDifferentiationFunction(interpolation_nodes, step),
            _ => null
        };
    }
}