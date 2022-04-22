using MathNet.Symbolics;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods.Newton;
public class NewtonMethod : ISolvingMethod
{
	IEnumerable<double> ISolvingMethod.Solve(NonLinearEquationsSystem system, double eps)
	{
		IEnumerable<SymbolicExpression>? variables = system.FunctionExpressions.First().CollectVariables();
		double[] x = { 0, 1 };
		double delta = double.MaxValue;
		List<double[]> results = new List<double[]>() { x };
		while (delta > eps)
		{
			Dictionary<string, FloatingPoint> values = new Dictionary<string, FloatingPoint>();
			for (int i = 0; i < x.Length; i++)
			{
				values.Add(variables.ElementAt(i).ToString(), x[i]);
			}
			var ys = functions.Select(function => function.Evaluate(values)).Select(fp => fp.RealValue).ToArray();
			var delta_x = Multiply(GetInverse(CalculateJacobi(functions, values)), GetNegativeVector(ys));
			delta = GetNorm(delta_x);
			x = GetSumVectors(x, delta_x);
			results.Add(x);
		}
		return results;
	}

	IEnumerable<IEnumerable<double>> ISolvingMethod.SolveWithSteps(NonLinearEquationsSystem system, double eps)
	{
		throw new NotImplementedException();
	}
}