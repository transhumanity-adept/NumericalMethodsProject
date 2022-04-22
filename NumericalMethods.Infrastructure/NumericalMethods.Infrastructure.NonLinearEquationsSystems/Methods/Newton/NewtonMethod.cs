using MathNet.Symbolics;

using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Shared;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods.Newton;
public class NewtonMethod : ISolvingMethod
{
	IEnumerable<double> ISolvingMethod.Solve(NonLinearEquationsSystem system, double eps)
	{
		IEnumerable<SymbolicExpression>? variables = system.FunctionExpressions.First().CollectVariables();
		double[] x = system.FunctionExpressions
			.Select(functionExpression => functionExpression.GetFreeMember().RealNumberValue)
			.ToArray();
		double delta = double.MaxValue;
		while (delta > eps)
		{
			Dictionary<string, FloatingPoint> values = new Dictionary<string, FloatingPoint>();
			for (int i = 0; i < x.Length; i++)
			{
				values.Add(variables.ElementAt(i).ToString(), x[i]);
			}

			var ys = system.FunctionExpressions
				.Select(function => function.Evaluate(values))
				.Select(fp => fp.RealValue)
				.ToArray();
			VectorColumn delta_x = SquareMatrix.CreateJacobiMatrix(system.FunctionExpressions, values).GetInverse() * -new VectorColumn(ys);
			delta = delta_x.GetNormM();
			x = (new VectorColumn(x) + delta_x).ToArray();
		}

		return x;
	}

	IEnumerable<IEnumerable<double>> ISolvingMethod.SolveWithSteps(NonLinearEquationsSystem system, double eps)
	{
		IEnumerable<SymbolicExpression>? variables = system.FunctionExpressions.First().CollectVariables();
		double[] x = system.FunctionExpressions
			.Select(functionExpression => functionExpression.GetFreeMember().RealNumberValue)
			.ToArray();
		double delta = double.MaxValue;
		List<double[]> results = new List<double[]>() { x };
		while (delta > eps)
		{
			Dictionary<string, FloatingPoint> values = new Dictionary<string, FloatingPoint>();
			for (int i = 0; i < x.Length; i++)
			{
				values.Add(variables.ElementAt(i).ToString(), x[i]);
			}

			var ys = system.FunctionExpressions
				.Select(function => function.Evaluate(values))
				.Select(fp => fp.RealValue)
				.ToArray();
			VectorColumn delta_x = SquareMatrix.CreateJacobiMatrix(system.FunctionExpressions, values).GetInverse() * -new VectorColumn(ys);
			delta = delta_x.GetNormM();
			x = (new VectorColumn(x) + delta_x).ToArray();
			results.Add(x);
		}

		return results;
	}
}