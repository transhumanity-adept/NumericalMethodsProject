using NumericalMethods.Approximation.Interpolations.Interfaces;
namespace NumericalMethods.Approximation.Interpolations.InterpolationFuncitons.Quadratic
{
    internal class QuadraticInterpolationFunction:IInterpolationFunction
    {
        private readonly IEnumerable<IInterpolationNode> _interpolation_nodes;
        public QuadraticInterpolationFunction()
        {
            
        }
        private List<QuadraticFunction> CreateFunctionsOfInterpolationIntervals()
        {
            List<QuadraticFunction> result = new List<QuadraticFunction>();
            List<IInterpolationNode> pointsForConstructingQuadraticFunction = new List<IInterpolationNode>();
            for (int i = 0; i < _interpolation_nodes.Count(); i++)
            {
                if (i + 2 >= _interpolation_nodes.Count())
                    break;
                pointsForConstructingQuadraticFunction.AddRange(new List<IInterpolationNode>() { _interpolation_nodes.ElementAt(i), _interpolation_nodes.ElementAt(i+1), _interpolation_nodes.ElementAt(i+2) });
                result.Add(MethodKrammer(CreateSystemOfEquations(pointsForConstructingQuadraticFunction)));
                pointsForConstructingQuadraticFunction.Clear();
            }
            return resu;

        }
        public double? Calculate(double argument)
        {
            List<double> coeffsOfQuadraticEquation;
            if (i + 2 >= Points.Count)
                break;
            pointsForConstructingQuadraticFunction.AddRange(new List<Point>() { Points[i], Points[i + 1], Points[i + 2] });
            coeffsOfQuadraticEquation = MethodKrammer(CreateSystemOfEquations(pointsForConstructingQuadraticFunction));
            if (i == 0)
                for (double x = pointsForConstructingQuadraticFunction.First().X; x < pointsForConstructingQuadraticFunction.Last().X; x += Step)
                {
                    intermediatePoints.Add(new Point(x, coeffsOfQuadraticEquation[0] * Math.Pow(x, 2) + coeffsOfQuadraticEquation[1] * x + coeffsOfQuadraticEquation[2]));
                }
            else
                for (double x = pointsForConstructingQuadraticFunction[1].X; x < pointsForConstructingQuadraticFunction.Last().X; x += Step)
                {
                    intermediatePoints.Add(new Point(x, coeffsOfQuadraticEquation[0] * Math.Pow(x, 2) + coeffsOfQuadraticEquation[1] * x + coeffsOfQuadraticEquation[2]));
                }

            return intermediatePoints;
        }

        private List<QuadraticFunction> CreateSystemOfEquations(List<IInterpolationNode> pointsForConstructingQuadraticFunction)
        {
            List<QuadraticFunction> quadraticEquations = new List<QuadraticFunction>();
            foreach (IInterpolationNode node in pointsForConstructingQuadraticFunction)
            {
                quadraticEquations.Add(new QuadraticFunction(Math.Pow(node.X, 2.0), node.X, 1, (double)node.Y));
            }

            return quadraticEquations;
        }

        private List<double> MethodKrammer(List<QuadraticFunction> quadraticFunction)
        {
            double determinant = FindDeterminantMatrix(FormingMatrix(quadraticFunction, 0));
            return new List<double>()
            {
                FindDeterminantMatrix(FormingMatrix(quadraticFunction,1))/determinant,
                FindDeterminantMatrix(FormingMatrix(quadraticFunction,2))/determinant,
                FindDeterminantMatrix(FormingMatrix(quadraticFunction,3))/determinant
            };
        }

        private List<List<double>> SplittingMatrix(List<List<double>> matrix, (int i, int j) indexDeleteElement)
        {
            List<List<double>> splittingMatrix = new List<List<double>>();
            for (int i = 0; i < matrix.Count; i++)
            {
                if (indexDeleteElement.i == i) continue;
                splittingMatrix.Add(new List<double>());
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    if (indexDeleteElement.j == j) continue;
                    splittingMatrix.Last().Add(matrix[i][j]);
                }
            }
            return splittingMatrix;
        }

        private double FindDeterminantMatrix(List<List<double>> matrixNDimensions)
        {
            if (matrixNDimensions.Count == 2)
                return matrixNDimensions[0][0] * matrixNDimensions[1][1] - matrixNDimensions[0][1] * matrixNDimensions[1][0];
            else
            {
                double[] determinantsMatrixNDimensions = new double[] { 1, 1, 1 };
                for (int j = 0; j < matrixNDimensions[0].Count; j++)
                {
                    determinantsMatrixNDimensions[j] = FindDeterminantMatrix(SplittingMatrix(matrixNDimensions, (0, j)));
                    if ((0 + 1 + j + 1) % 2 == 0)
                        determinantsMatrixNDimensions[j] *= matrixNDimensions[0][j];
                    else
                        determinantsMatrixNDimensions[j] *= -matrixNDimensions[0][j];

                }
                return determinantsMatrixNDimensions.Sum();
            }
        }

        private List<List<double>> FormingMatrix(List<QuadraticFunction> quadraticFunction, int indexDeterminant)
        {
            List<List<double>> matrix = new List<List<double>>();
            quadraticFunction.ForEach(quadraticEquation =>
            {
                matrix.Add(new List<double>());
                switch (indexDeterminant)
                {
                    case 0:
                        {
                            matrix.Last().Add(quadraticEquation.A);
                            matrix.Last().Add(quadraticEquation.B);
                            matrix.Last().Add(quadraticEquation.C);
                            break;
                        }
                    case 1:
                        {
                            matrix.Last().Add(quadraticEquation.Y);
                            matrix.Last().Add(quadraticEquation.B);
                            matrix.Last().Add(quadraticEquation.C);
                            break;
                        }
                    case 2:
                        {
                            matrix.Last().Add(quadraticEquation.A);
                            matrix.Last().Add(quadraticEquation.Y);
                            matrix.Last().Add(quadraticEquation.C);
                            break;
                        }
                    case 3:
                        {
                            matrix.Last().Add(quadraticEquation.A);
                            matrix.Last().Add(quadraticEquation.B);
                            matrix.Last().Add(quadraticEquation.Y);
                            break;
                        }
                }
            });
            return matrix;
        }
    }
}
}
