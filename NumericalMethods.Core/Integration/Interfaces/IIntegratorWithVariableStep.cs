﻿namespace NumericalMethods.Core.Integration.Interfaces;
public interface IIntegratorWithVariableStep
{
    public double Integrate(double start, double end, int count_nodes);
}