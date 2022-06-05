using MathNet.Symbolics;
using NumericalMethods.Console;
using NumericalMethods.Core.CauchyProblem;
using NumericalMethods.Core.CauchyProblem.Interfaces;

Console.WriteLine($"{nameof(OneStepMethods.RungeKuttaFourthOrder)}:");
CauchyProblemBuilder.BuildWithOneStep("2*x", OneStepMethods.RungeKuttaThridOrder)
    .Calculate(
        b: 10,
        h: 1,
        initialGuess: (x: 1, ys: new Dictionary<string, double>()
        {
            { "y0", 1 }
        })
    ).GetRows(..).ForEach(rows =>
    {
        rows.ToList().ForEach(row => Console.Write($"{row.Key} : {row.Value}  "));
        Console.WriteLine();
    });

Console.WriteLine();
Console.WriteLine($"{nameof(MultiStepMethods.AdamsBashforth)}:");
CauchyProblemBuilder.BuildWithMultiStep("2*x", MultiStepMethods.AdamsMoulton, OneStepMethods.RungeKuttaThridOrder, 3)
    .Calculate(
        b: 10,
        h: 1,
        initialGuess: (x: 1, ys: new Dictionary<string, double>()
        {
            { "y0", 1 }
        })
    ).GetRows(..).ForEach(rows =>
    {
        rows.ToList().ForEach(row => Console.Write($"{row.Key} : {row.Value}  "));
        Console.WriteLine();
    });

Console.WriteLine();
Console.WriteLine($"Adams:");
CauchyProblemBuilder.CreateAdams("2*x", OneStepMethods.RungeKuttaThridOrder)
    .Calculate(
        b: 10,
        h: 1,
        initialGuess: (x: 1, ys: new Dictionary<string, double>()
        {
            { "y0", 1 }
        })
    ).GetRows(..).ForEach(rows =>
    {
        rows.ToList().ForEach(row => Console.Write($"{row.Key} : {row.Value}  "));
        Console.WriteLine();
    });