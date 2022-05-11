using MathNet.Symbolics;
using NumericalMethods.Console;

int _nodes_count = 4;
int _derrivative_degree = 2;

SymbolicExpression[] ys = new SymbolicExpression[_nodes_count];
SymbolicExpression[] ysLastDerivative = new SymbolicExpression[_nodes_count];
SymbolicExpression[] ysBeforeLastDerivative = new SymbolicExpression[_nodes_count];
for (int i = 0; i < _nodes_count; i++)
{
    ys[i] = $"(x - x0)^{i}";
}

for (int i = 0; i < _derrivative_degree - 1; i++)
{
    ys = ys.Select(y => y.Differentiate("x")).ToArray();
}

ysBeforeLastDerivative = ys;
ysLastDerivative = ys.Select(y => y.Differentiate("x")).ToArray();

TestFactorial.Run();
TestNewtonDerrivative.Run();
TestIntegration.Run();
TestDifferentialEquations.Run();