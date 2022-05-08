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
        private readonly double[] _h;
        public UndefinedCoefficientsDifferentiationFunction(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degree) 
            : base(differentiationNodes, step, derrivative_degree)
        {
            _matrix_size = _nodes.Count();
            _h = differentiationNodes.Select(node => node.X - _first_node.X).ToArray();
        }

        public double? Calculate(double argument)
        {
            throw new NotImplementedException();
        }

        private VectorColumn SolveLinearSystem(SquareMatrix system, VectorColumn b) => system.Invert() * b;

        private VectorColumn CreateVectorB()
        {
            return new VectorColumn(Enumerable.Range(0, _matrix_size).Select(value => (double)value).ToArray());
        }

        private SquareMatrix CreateMatrix()
        {
            double[,] matrixBody = new double[_matrix_size, _matrix_size];
            for (int j = 0; j < _matrix_size; j++)
            {
                matrixBody[0, j] = 1;
            }
            for (int i = 1; i < _matrix_size; i++)
            {
                for (int j = 0; j < _matrix_size; j++)
                {
                    matrixBody[i, j] = _h[i] * Math.Pow(j, i);
                }
            }
            return new SquareMatrix(matrixBody);
        }
    }
}
