using NumericalMethods.Approximation.Interpolations.Interfaces;
namespace NumericalMethods.WPFApplication;
public record class InterpolationNode(double X, double Y) : IInterpolationNode;