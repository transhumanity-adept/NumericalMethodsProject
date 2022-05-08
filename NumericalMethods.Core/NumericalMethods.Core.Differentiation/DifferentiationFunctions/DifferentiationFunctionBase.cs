using NumericalMethods.Core.Approximation;
using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions
{
    internal abstract class DifferentiationFunctionBase
    {
        protected readonly double _step;
        protected readonly int _derrivative_degree;
        protected readonly IInterpolationFunction _interpolation_function;
        protected readonly IDifferentiationNode _center_node;
        protected readonly IDifferentiationNode _first_node;
        protected readonly IDifferentiationNode _last_node;
        public DifferentiationFunctionBase(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degree)
        {
            _step = step;
            _derrivative_degree = derrivative_degree;
            _first_node = differentiationNodes.First();
            _last_node = differentiationNodes.Last();
            _center_node = differentiationNodes.OrderBy((node) => node.X).ElementAt(differentiationNodes.Count() / 2);
            IEnumerable<IInterpolationNode> mapped_nodes = differentiationNodes.Select(node => new InterpolationNode(node.X, node.Y));
            _interpolation_function = InterpolationBuilder.Build(mapped_nodes, InterpolationFunctionType.Cubic);
        }
        /*protected double? GetFiniteDifference(double argument,int degree)
        {
            bool isLeft = argument > centerNode.X;
            return GetFiniteDifferenceRecursive(argument, 1, degree, isLeft);
        }
        private double? GetFiniteDifferenceRecursive(double current_argument, int depth, int degree, bool isLeft)
        {
            if (depth == degree)
                return isLeft ? GetLeftFiniteDifference(current_argument)
                : GetRightFiniteDifference(current_argument);
            else
            {
                int new_depth = depth + 1;
                return isLeft ?
                    GetFiniteDifferenceRecursive(current_argument, new_depth, degree, isLeft) - GetFiniteDifferenceRecursive(current_argument - _step, new_depth, degree, isLeft)
                    : GetFiniteDifferenceRecursive(current_argument + _step, new_depth, degree, isLeft) - GetFiniteDifferenceRecursive(current_argument, new_depth, degree, isLeft);
            }
        }*/
        
        private double? GetCenterFiniteDifference(double argument) => _interpolation_function.Calculate(argument + _step) - _interpolation_function.Calculate(argument - _step);
        protected double? GetCenterFiniteDifference(double argument, int degree) => GetCenterFiniteDifferenceRecursive(argument, degree, 1);
		private double? GetCenterFiniteDifferenceRecursive(double argument, int degree, int depth)
		{
            return depth == degree ?
                GetCenterFiniteDifference(argument)
                : GetCenterFiniteDifferenceRecursive(argument + _step, degree, depth + 1) - GetCenterFiniteDifferenceRecursive(argument - _step, degree, depth + 1);

        }

		private double? GetLeftFiniteDifference(double argument) => _interpolation_function.Calculate(argument) - _interpolation_function.Calculate(argument - _step);
        protected double? GetLeftFiniteDifference(double argument, int degree) => GetLeftFiniteDifferenceRecursive(argument, degree, 1);
		private double? GetLeftFiniteDifferenceRecursive(double argument, int degree, int depth)
		{
            return depth == degree ?
                GetLeftFiniteDifference(argument)
                : GetLeftFiniteDifferenceRecursive(argument, degree, depth + 1) - GetLeftFiniteDifferenceRecursive(argument - _step, degree, depth + 1);
		}

		private double? GetRightFiniteDifference(double argument) => _interpolation_function.Calculate(argument + _step) - _interpolation_function.Calculate(argument);
        protected double? GetRightFiniteDifference(double argument, int degree) => GetRightFiniteDifferenceRecursive(argument, degree, 1);
        private double? GetRightFiniteDifferenceRecursive(double current_argument, int degree, int depth)
		{
            return depth == degree ?
                GetRightFiniteDifference(current_argument)
                : GetRightFiniteDifferenceRecursive(current_argument + _step, degree, depth + 1) - GetRightFiniteDifferenceRecursive(current_argument, degree, depth + 1);
		}
    }
}
