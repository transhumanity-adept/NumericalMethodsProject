﻿

namespace NumericalMethods.Core.Integration.Interfaces
{
    public interface IDerivativesFinder
    {
        public IFunction CalculateDerivative(IFunction function, int order);
    }
}