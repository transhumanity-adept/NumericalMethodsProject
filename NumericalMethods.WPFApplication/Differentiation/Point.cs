using NumericalMethods.Approximation.Interpolations.Interfaces;
using NumericalMethods.Differentiations.Interfaces;

namespace NumericalMethods.WPFApplication.Differentiation;
public record class Point(double X, double Y) : IInterpolationNode, IDifferentiationNode;