using NumericalMethods.Core.Differentiations.DifferentiationFunctions;
using NumericalMethods.Core.Differentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Core.Differentiation.DifferentiationFunctions.Runge
{
    internal class RungeDifferentiationFunction : DifferentiationFunctionBase, IDifferentiationFunction
    {
        private readonly int _accuracy_order;
        private readonly int _number_of_used_points;
        public RungeDifferentiationFunction(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degree, int accuracy_order, int number_of_used_points) 
            : base(differentiationNodes, step, derrivative_degree)
        {
            _accuracy_order = accuracy_order;
            _number_of_used_points = number_of_used_points;
        }

        public double? Calculate(double argument)
        {
            Func<double, double?> f = _interpolation_function.Calculate;
            double? xh = (f(argument) - f(argument - _step)) / _step;
            double? xkh = (f(argument) - f(argument - _step * _number_of_used_points)) / (_step * _number_of_used_points);
            return xh + (xh - xkh) / (Math.Pow(_number_of_used_points, _accuracy_order) - 1);
        }
    }
}
