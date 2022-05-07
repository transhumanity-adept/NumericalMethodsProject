using NumericalMethods.Core.Differentiations.Interfaces;


namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions.NewtonPolynomial
{
    internal class NewtonPolynomialDifferentiationFunction : DifferentiationFunctionBase, IDifferentiationFunction
    {
        private readonly int dimension;
        public NewtonPolynomialDifferentiationFunction(IEnumerable<IDifferentiationNode> differentiationNodes, double step,int dimension) : base(differentiationNodes, step)
        {
            this.dimension = dimension;
        }
        private Func<double,double?> BuildNewtonPolynomial(int dimension)
        {
            return new Func<double, double?>((double x) =>
            {
                double? result = 0;
                for (int i = 0; i < dimension; i++)
                {
                    result += x <= centerNode.X ? (1 - i + 1) / CalculateFactorial(i)//В данном случае q = 1,мейби расписать
                    : (-1 - i + 1) / CalculateFactorial(i) * GetFiniteDifference(x, i);
                }
                return result;
            });
        }
        private int CalculateFactorial(int value)
        {
            if (value <= 0) return 0;
            int factorialValue = 1;
            for (int i = 1; i <= value; i++)
            {
                factorialValue *= i;
            }
            return factorialValue;
        }
        public double? Calculate(double argument, int derivative_degree)
        {
            throw new NotImplementedException();
        }
    }
}
