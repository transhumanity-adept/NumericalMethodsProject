using NumericalMethods.Approximation.Interpolations.Interfaces;

namespace NumericalMethods.Differentiations;
public record class InterpolationNode(double X, double Y) : IInterpolationNode;
