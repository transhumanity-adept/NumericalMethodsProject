using NumericalMethods.Core.Approximation.Interfaces;

namespace NumericalMethods.Core.Differentiations;
public record class InterpolationNode(double X, double Y) : IInterpolationNode;
