﻿namespace NumericalMethods.Approximation.Interpolations.InterpolationFuncitons.Quadratic
{
    internal record class QuadraticFunction(double A,double B, double C,double Y)
    {
        public double Calculate(double argument) => A * Math.Pow(argument, 2) + B * argument + C;
    }

}
