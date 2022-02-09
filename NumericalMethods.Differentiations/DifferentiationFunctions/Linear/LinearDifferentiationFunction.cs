using NumericalMethods.Approximation.Interpolations;
using NumericalMethods.Approximation.Interpolations.Interfaces;
using NumericalMethods.Differentiations.Interfaces;

namespace NumericalMethods.Differentiations.DifferentiationFunctions.Linear
{
    public class LinearDifferentiationFunction : IDifferentiationFunction
    {
        private readonly IEnumerable<IDifferentiationNode> _interpolation_nodes;
        private readonly double _step;
        private readonly IInterpolationFunction _interpolation_function;
        public LinearDifferentiationFunction(IEnumerable<IDifferentiationNode> interpolation_nodes, double step)
        {
            _interpolation_nodes = interpolation_nodes;
            _step = step;
            IEnumerable<IInterpolationNode> mapped_nodes = interpolation_nodes.Select(node => new InterpolationNode(node.X, node.Y));
            _interpolation_function = InterpolationBuilder.Build(mapped_nodes, InterpolationFunctionType.Linear);
        }
        public double? Calculate(double argument, int derivative_degree)
        {
            return CalculateDerivativeRecursive(argument, 1, derivative_degree);
        }
        private double? CalculateDerivativeRecursive(double current_x, int depth, int order_number)
        {
            var left_y = _interpolation_function.Calculate(current_x - _step);
            var right_y = _interpolation_function.Calculate(current_x + _step);
            var new_deph = depth + 1;
            return depth == order_number
                ? (right_y - left_y) / (2 * _step)
                : (CalculateDerivativeRecursive(current_x + _step, new_deph, order_number) - CalculateDerivativeRecursive(current_x - _step, new_deph, order_number)) / (2 * _step);
        }
    }
}
