using NumericalMethods.Core.Approximation;
using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions
{
    internal abstract class DifferentiationFunctionBase
    {
        protected readonly double step;
        protected readonly IInterpolationFunction interpolationFunction;
        protected readonly IDifferentiationNode centerNode;
        protected readonly IDifferentiationNode _first_node;
        protected readonly IDifferentiationNode _last_node;
        public DifferentiationFunctionBase(IEnumerable<IDifferentiationNode> differentiationNodes, double step)
        {
            this.step = step;
            _first_node = differentiationNodes.First();
            _last_node = differentiationNodes.Last();
            centerNode = differentiationNodes.OrderBy((node) => node.X).ElementAt(differentiationNodes.Count() / 2);
            IEnumerable<IInterpolationNode> mapped_nodes = differentiationNodes.Select(node => new InterpolationNode(node.X, node.Y));
            interpolationFunction = InterpolationBuilder.Build(mapped_nodes, InterpolationFunctionType.Quadratic);
        }
        protected double? GetFiniteDifference(double argument,int degree)
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
                    GetFiniteDifferenceRecursive(current_argument, new_depth, degree, isLeft) - GetFiniteDifferenceRecursive(current_argument - step, new_depth, degree, isLeft)
                    : GetFiniteDifferenceRecursive(current_argument + step, new_depth, degree, isLeft) - GetFiniteDifferenceRecursive(current_argument, new_depth, degree, isLeft);
            }
        }
        
        
        private double? GetCenterFiniteDifference(double argument) => interpolationFunction.Calculate(argument + step) - interpolationFunction.Calculate(argument - step);
        protected double? GetCenterFiniteDifference(double argument, int degree) => GetCenterFiniteDifferenceRecursive(argument, degree, 1);
		private double? GetCenterFiniteDifferenceRecursive(double argument, int degree, int depth)
		{
            return depth == degree ?
                GetCenterFiniteDifference(argument)
                : GetCenterFiniteDifferenceRecursive(argument + step, degree, depth + 1) - GetCenterFiniteDifferenceRecursive(argument - step, degree, depth + 1);

        }

		private double? GetLeftFiniteDifference(double argument) => interpolationFunction.Calculate(argument) - interpolationFunction.Calculate(argument - step);
        protected double? GetLeftFiniteDifference(double argument, int degree) => GetLeftFiniteDifferenceRecursive(argument, degree, 1);
		private double? GetLeftFiniteDifferenceRecursive(double argument, int degree, int depth)
		{
            return depth == degree ?
                GetLeftFiniteDifference(argument)
                : GetLeftFiniteDifferenceRecursive(argument, degree, depth + 1) - GetLeftFiniteDifferenceRecursive(argument - step, degree, depth + 1);
		}

		private double? GetRightFiniteDifference(double argument) => interpolationFunction.Calculate(argument + step) - interpolationFunction.Calculate(argument);
        protected double? GetRightFiniteDifference(double argument, int degree) => GetRightFiniteDifferenceRecursive(argument, degree, 1);
        private double? GetRightFiniteDifferenceRecursive(double current_argument, int degree, int depth)
		{
            return depth == degree ?
                GetRightFiniteDifference(current_argument)
                : GetRightFiniteDifferenceRecursive(current_argument + step, degree, depth + 1) - GetRightFiniteDifferenceRecursive(current_argument, degree, depth + 1);
		}
    }
}
