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
        public DifferentiationFunctionBase(IEnumerable<IDifferentiationNode> differentiationNodes, double step)
        {
            this.step = step;
            centerNode = differentiationNodes.OrderBy((node) => node.X).ElementAt(differentiationNodes.Count() / 2);
            IEnumerable<IInterpolationNode> mapped_nodes = differentiationNodes.Select(node => new InterpolationNode(node.X, node.Y));
            interpolationFunction = InterpolationBuilder.Build(mapped_nodes, InterpolationFunctionType.Quadratic);
        }
        protected double? GetFiniteDifference(double argument,int degree)
        {
            bool isLeft = argument <= centerNode.X ? false : true;
            return GetFiniteDifferenceRecursive(argument, 1, degree,isLeft);
        }
        private double? GetFiniteDifferenceRecursive(double current_argument,int depth,int degree,bool isLeft)
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
        private double? GetLeftFiniteDifference(double argument)
        {
            return interpolationFunction.Calculate(argument) - interpolationFunction.Calculate(argument - step);
        }
        private double? GetRightFiniteDifference(double argument)
        {
            return interpolationFunction.Calculate(argument + step) - interpolationFunction.Calculate(argument);
        }
    }
}
