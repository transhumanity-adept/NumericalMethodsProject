using NumericalMethods.Core.NonLinearEquationsSystems.Interfaces;
using NumericalMethods.Core.NonLinearEquationsSystems.Methods;

namespace NumericalMethods.Core.NonLinearEquationsSystems;
public record class NonLinearEquationsSystemsSolver(ISolvingMethod SolvingMethod) : INonLinearEquationsSystemsSolver
{
    public IEnumerable<double> Solve(NonLinearEquationsSystem system, double eps)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IEnumerable<double>> solveWithSteps(NonLinearEquationsSystem system, double eps)
    {
        throw new NotImplementedException();
    }
}
