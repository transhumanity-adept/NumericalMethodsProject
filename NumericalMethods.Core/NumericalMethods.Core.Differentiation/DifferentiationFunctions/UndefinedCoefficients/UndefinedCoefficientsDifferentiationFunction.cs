using NumericalMethods.Core.Differentiations.DifferentiationFunctions;
using NumericalMethods.Core.Differentiations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Core.Differentiation.DifferentiationFunctions.UndefinedCoefficients
{
    public class UndefinedCoefficientsDifferentiationFunction : DifferentiationFunctionBase, IDifferentiationFunction
    {
        private readonly int _matrix_size;
        public UndefinedCoefficientsDifferentiationFunction(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degree) 
            : base(differentiationNodes, step, derrivative_degree)
        {
            _matrix_size = _nodes.Count();
        }

        public double? Calculate(double argument)
        {
            throw new NotImplementedException();
        }

        private VectorColumn SolveLinearSystem(SquareMatrix system, VectorColumn b) => system.Invert() * b;

        private VectorColumn CreateVectorB()
        {
            return new VectorColumn(Enumerable.Range(0, _matrix_size).ToArray());
        }
    }
}
