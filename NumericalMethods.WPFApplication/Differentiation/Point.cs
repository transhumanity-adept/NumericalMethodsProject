using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.WPFApplication.Differentiation;
public record class Point(double X, double Y) : IInterpolationNode, IDifferentiationNode;