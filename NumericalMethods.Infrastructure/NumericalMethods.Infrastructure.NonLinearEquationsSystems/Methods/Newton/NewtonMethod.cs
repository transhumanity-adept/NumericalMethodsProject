namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods.Newton;
public class NewtonMethod : ISolvingMethod
{
    IEnumerable<double> ISolvingMethod.Solve(NonLinearEquationsSystem system, double eps)
    {
        throw new NotImplementedException();
    }

    IEnumerable<IEnumerable<double>> ISolvingMethod.SolveWithSteps(NonLinearEquationsSystem system, double eps)
    {
        throw new NotImplementedException();
    }
}
