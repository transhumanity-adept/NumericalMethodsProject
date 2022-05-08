﻿using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Core.Differentiations.Interfaces;

namespace NumericalMethods.Core.Differentiations.DifferentiationFunctions.Linear;
internal class LinearDifferentiationFunction : DifferentiationFunctionBase, IDifferentiationFunction
{
    private readonly INewtonDifferentiationFunction _newton_funtion;
	public LinearDifferentiationFunction(IEnumerable<IDifferentiationNode> interpolation_nodes, double step, int derrivative_degree)
        :base(interpolation_nodes, step, derrivative_degree)
	{
        _newton_funtion = DifferentiationBuilder.CreateNewton(interpolation_nodes, step, derrivative_degree, derrivative_degree + 1);
	}

    public double? Calculate(double argument) => _newton_funtion.Calculate(argument);
}
