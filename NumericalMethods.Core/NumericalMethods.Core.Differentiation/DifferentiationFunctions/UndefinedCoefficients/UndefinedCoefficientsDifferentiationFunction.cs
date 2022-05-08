using NumericalMethods.Core.Differentiations.DifferentiationFunctions;
using NumericalMethods.Core.Differentiations.Interfaces;
using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Core.Differentiation.DifferentiationFunctions.UndefinedCoefficients;

namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions.UndefinedCoefficients
{
    internal class UndefinedCoefficientsDifferentiationFunction : DifferentiationFunctionBase, IUndefinedCoefficientsDifferentiationFunction
    {
        private readonly int _nodes_count;
        private readonly double[] _h;
        private string[] _old_derivative = null, _new_derivative = null;
        public UndefinedCoefficientsDifferentiationFunction(IEnumerable<IDifferentiationNode> differentiationNodes, double step, int derrivative_degree) 
            : base(differentiationNodes, step, derrivative_degree)
        {
            _nodes_count = _nodes.Count();
            _h = differentiationNodes.Select(node => node.X - _first_node.X).ToArray();
        }

        public IEnumerable<DifferentiationNode> Calculate()
        {
            VectorColumn vector = null;
            for (int i = 0; i < _derrivative_degree; i++)
                vector = SolveLinearSystem(СompilationSystemEquations());
            List<DifferentiationNode> result = new List<DifferentiationNode>();
            double y = 0;
            for (int i = 1; i < _nodes_count;i++)
            {
                for (int j = 0; j < _nodes_count; j++)
                {
                    if (_nodes.ElementAtOrDefault(j + i - 1) == null) break;
                    y += _nodes.ElementAt(j+i-1).Y * vector[j];
                }
                result.Add(new DifferentiationNode(_nodes.ElementAt(i).X, y));
                y = 0; 
            }
            return result;
        }

        private VectorColumn SolveLinearSystem(IEnumerable<LinerEquation> linerEquations) => CreateMatrix(linerEquations).Invert() * CreateVectorB(linerEquations);

        private VectorColumn CreateVectorB(IEnumerable<LinerEquation> linerEquations) => new VectorColumn(linerEquations.Select(element => element.FreeMember).ToArray());
        private void Find_new_derivarive()
        {
            _old_derivative = _new_derivative;
            if(_old_derivative == null)
            {
                _old_derivative = new string[_nodes.Count()];
                for (int i = 0; i < _old_derivative.Length; i++)
                {
                    _old_derivative[i] = $"h^{i}";
                }
            }
            _new_derivative = new string[_old_derivative.Length];
            for (int i = 0; i < _old_derivative.Length; i++)
            {
                _new_derivative[i] = SymbolicExpression.Parse(_old_derivative[i]).Expand().Differentiate("h").ToString();
            }
        }
        private IEnumerable<LinerEquation> СompilationSystemEquations()
        {
            Find_new_derivarive();
            List<LinerEquation> result = new List<LinerEquation>();
            List<double> coef = new List<double>();
            for (int i = 0; i < _old_derivative.Length; i++)
            {
                for (int j = 0; j < _old_derivative.Length; j++)
                {
                    coef.Add(SymbolicExpression.Parse(_old_derivative[i]).Evaluate(new Dictionary<string, FloatingPoint>() { { "h", _h[j] } }).RealValue);
                }
                result.Add(new LinerEquation(coef, SymbolicExpression.Parse(_new_derivative[i]).Evaluate(new Dictionary<string, FloatingPoint>() { { "h", _h[1] } }).RealValue));
                coef = new List<double>();
            }
            return result;
        }

        private SquareMatrix CreateMatrix(IEnumerable<LinerEquation> linerEquations)
        {
            double[,] matrixBody = new double[_nodes_count, _nodes_count];
            for (int i = 0; i < _nodes_count; i++)
            {
                for (int j = 0; j < _nodes_count; j++)
                {
                    matrixBody[i,j] = linerEquations.ElementAt(i).Coefficients.ElementAt(j);
                }
            }
            return new SquareMatrix(matrixBody);
        }


    }
}
