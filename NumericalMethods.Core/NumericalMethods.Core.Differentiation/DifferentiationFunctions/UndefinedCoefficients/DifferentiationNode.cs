using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiation.DifferentiationFunctions.UndefinedCoefficients
{
    public class DifferentiationNode : IDifferentiationNode
    {
        public double X { get; init; }
        public double Y { get; init; }
        public DifferentiationNode(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
