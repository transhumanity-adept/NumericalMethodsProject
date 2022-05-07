using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions.Quadratic
{
    internal class QuadraticDifferentationFunction : DifferentiationFunctionBase,IDifferentiationFunction
    {
        public QuadraticDifferentationFunction(IEnumerable<IDifferentiationNode> differentiationNodes,double step):base(differentiationNodes,step)
        {}
        public double? Calculate(double argument, int derivative_degree)
        {
            return CalculateRecursive(argument, 1, derivative_degree);
        }
        public double? CalculateRecursive(double argument,int depth,int derivative_degree)
        {   
            double? right_y,deltY1, deltaLeft_y, deltaRight_y, deltY2;
            deltY1 = GetFiniteDifference(argument,1);
            deltY2 = GetFiniteDifference(argument, 2);
            //if (interpolationFunction.Calculate(argument + step) == null)
            //{
            //    deltY1 = interpolationFunction.Calculate(argument) - interpolationFunction.Calculate(argument - step);
            //    deltaLeft_y = interpolationFunction.Calculate(argument - step) - interpolationFunction.Calculate(argument - 2 * step);
            //    deltaRight_y = deltY1;
            //    deltY2 = deltaRight_y - deltaLeft_y;
            //}
            //else
            //{

            //}

            var new_deph = depth + 1;
            if (depth != derivative_degree)
            {
                deltY1 = GetFiniteDifference(argument, 1);
                deltY2 = GetFiniteDifference(argument, 2);
                right_y = CalculateRecursive(argument + step, new_deph, derivative_degree);
                deltY1 = right_y - argument;
                deltaLeft_y = deltY1;
                deltaRight_y = CalculateRecursive(right_y.Value + step, new_deph, derivative_degree) - right_y;
                deltY2 = deltaRight_y - deltaLeft_y;
            }
            return argument <= centerNode.X ? (deltY1 - 0.5 * deltY2) / step : (deltY1 + 0.5 * deltY2) / step;

        }
    }
}
