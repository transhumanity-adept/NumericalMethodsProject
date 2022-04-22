using MathNet.Symbolics;

using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Exceptions;

using System.Runtime.CompilerServices;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems
{
	public class NonLinearEquationsSystem
	{
		public NonLinearEquationsSystem(params string[] functions)
		{
			Functions = functions;
			FunctionExpressions = functions
				.Select(function => SymbolicExpression.Parse(function))
				.ToList();
			CheckInvariants();
		}

		private void CheckInvariants()
		{
			InvariantException.ThrowIf(
				isViolated: CheckFunctionsIsNotNull(),
				message: $"{nameof(Functions)} не должно быть null");
			InvariantException.ThrowIf(
				isViolated: CheckFunctionsHaveAnyEquation(),
				message: $"{nameof(Functions)} должно иметь хотябы одно уравнение");
			InvariantException.ThrowIf(
				isViolated: CheckNumberVariablesInEquationsIsSame(),
				message: $"{nameof(Functions)} должны иметь одинаковое количество аргументов");
			InvariantException.ThrowIf(
				isViolated: CheckNumberVariablesEqualsNumberEquations(),
				message: $"Количество аргументов в {nameof(Functions)} должно совпадать с количеством уравнений");
		}

		private bool CheckFunctionsIsNotNull()
		{
			return Functions is null;
		}

		private bool CheckFunctionsHaveAnyEquation()
		{
			return Functions.Length == 0;
		}

		private bool CheckNumberVariablesInEquationsIsSame()
		{
			int count_variables_in_first_equation = FunctionExpressions.First().CollectVariables().Count();
			return FunctionExpressions
				.Skip(1)
				.All(functionExpression => functionExpression.CollectVariables().Count() == count_variables_in_first_equation);
		}

		private bool CheckNumberVariablesEqualsNumberEquations()
		{
			return FunctionExpressions.First().CollectVariables().Count() == FunctionExpressions.Count(); 
		}

		public string[] Functions { get; }
		public IEnumerable<SymbolicExpression> FunctionExpressions { get; }
	}
}
