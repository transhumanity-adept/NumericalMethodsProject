﻿namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Interfaces;
public interface INonLinearEquationsSystemsSolver
{
	public IEnumerable<double> Solve(NonLinearEquationsSystem system, double eps);
	public IEnumerable<IEnumerable<double>> SolveWithSteps(NonLinearEquationsSystem system, double eps);
}
