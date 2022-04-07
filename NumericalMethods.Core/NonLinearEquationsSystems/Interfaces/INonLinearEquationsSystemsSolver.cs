namespace NumericalMethods.Core.NonLinearEquationsSystems.Interfaces;
public interface INonLinearEquationsSystemsSolver
{
    public IEnumerable<double> Solve(NonLinearEquationsSystem system, double eps);
    public IEnumerable<IEnumerable<double>> solveWithSteps(NonLinearEquationsSystem system, double eps);
}
