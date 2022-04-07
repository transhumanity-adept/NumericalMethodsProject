using MathNet.Symbolics;

using org.mariuszgromada.math.mxparser;

using System.Data.Common;
using System.Drawing;

using FuctionMX = org.mariuszgromada.math.mxparser.Function;
using FunctionS = MathNet.Symbolics.Function;

//FuctionMX function = new FuctionMX("f(x) = x^2");
//double a = -5;
//double b = 5;
//int n = 9;

//Console.WriteLine($"S = {GaussIntegration(function, a, b, n)}");

//#region Gauss
//double GaussIntegration(FuctionMX function, double a, double b, int n)
//{
//    double[] acoef = new double[n];
//    double[] x = new double[n];
//    for (int i = 1; i <= n; i++)
//    {
//        double ti = T(n - 1, i, n);
//        double pd = PD(n, ti);
//        double diva = (1 - ti * ti) * pd * pd;
//        acoef[i - 1] = 2 / diva;
//        x[i - 1] = GetRealX(a, b, ti);
//    }

//    return (b - a) / 2 * acoef.Zip(x, (ac, xc) => ac * function.calculate(xc)).Sum();
//}

//double GetRealX(double a, double b, double t)
//{
//    return (a + b) / 2 + (b - a) / 2 * t;
//}

//double T(int k, int i, int n)
//{
//    if(k == 0)
//    {
//        double t1 = Math.PI * (4 * i - 1);
//        double t2 = 4 * n + 2;
//        return Math.Cos(t1 / t2);
//    }
//    double tprev = T(k - 1, i, n);
//    return tprev - P(n, tprev) / PD(n, tprev); 
//}

//double P(double n, double t)
//{
//    return n switch
//    {
//        0 => 1,
//        1 => t,
//        _ => (2 * (n - 1) + 1) / ((n - 1) + 1) * t * P(n - 1, t) 
//             - (n - 1) / ((n - 1) + 1) * P(n - 2, t)
//    };
//}

//double PD(double n, double t)
//{
//    double left = n / (1 - t * t);
//    double right = P(n - 1, t) - t * P(n, t);
//    return left * right;
//}
//#endregion

//#region Chebyshev

//var func = SymbolicExpression.Parse("x1^2 + x2^2 - 9");
//var x = func.CollectVariables();
//var res = func.Evaluate(new Dictionary<string, FloatingPoint>() { { "x1", 2 }, { "x2", 3 } });

//var part_der = func.Differentiate(x.Last());
//var res_der = part_der.Evaluate(new Dictionary<string, FloatingPoint>() { { "x2", 2 } });

//var value = res_der.RealValue;
//#endregion

#region SNU
List<SymbolicExpression> exps = new List<SymbolicExpression>()
{
    SymbolicExpression.Parse("x1 + x2 - 3"),
    SymbolicExpression.Parse("x1^2 + x2^2 - 9")
};

Dictionary<string, FloatingPoint> dict = new Dictionary<string, FloatingPoint>()
{
    { "x1", 1 },
    { "x2", 5 }
};

var res = Newton(exps, 0.001);

var x = 0;

List<double[]> Newton(List<SymbolicExpression> functions, double eps)
{
    var variables = functions.First().CollectVariables();
    //double[] x = Enumerable.Repeat(1.0, variables.Count()).ToArray();
    double[] x = { 0, 1 };
    double delta = double.MaxValue;
    List<double[]> results = new List<double[]>() { x };
    while(delta > eps)
    {
        Dictionary<string, FloatingPoint> values = new Dictionary<string, FloatingPoint>();
        for (int i = 0; i < x.Length; i++)
        {
            values.Add(variables.ElementAt(i).ToString(), x[i]);
        }
        var ys = functions.Select(function => function.Evaluate(values)).Select(fp => fp.RealValue).ToArray();
        var delta_x = Multiply(GetInverse(CalculateJacobi(functions, values)), GetNegativeVector(ys));
        delta = GetNorm(delta_x);
        x = GetSumVectors(x, delta_x);
        results.Add(x);
    }
    return results;
}

double GetNorm(double[] vector)
{
    return vector.Select(vector => Math.Abs(vector)).Max();
}

double[] Multiply(double[,] matrix, double[] vector)
{
    double[] result = new double[vector.Length];
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        double sum = 0;
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            sum += matrix[i, j] * vector[j];
        }
        result[i] = sum;
    }
    return result;
}

double[] GetNegativeVector(double[] vector)
{
    return vector.Select(v => -v).ToArray();
}

double[] GetSumVectors(double[] v1, double[] v2)
{
    return v1.Zip(v2, (v1, v2) => v1 + v2).ToArray();
}

double[,] CalculateJacobi(List<SymbolicExpression> functions, Dictionary<string, FloatingPoint> values)
{
    var variables = functions.First().CollectVariables();
    int count_params = variables.Count();
    double[,] result = new double[count_params, count_params];
    for (int i = 0; i < result.GetLength(0); i++)
    {
        for (int j = 0; j < result.GetLength(1); j++)
        {
            result[i, j] = functions[i].Differentiate(variables.ElementAt(j)).Evaluate(new Dictionary<string, FloatingPoint>() { { variables.ElementAt(j).ToString(), values[variables.ElementAt(j).ToString()] } }).RealValue;
        }
    }

    return result;
}

double[,] GetMatrixWithout(double[,] matrix, int index_row, int index_column)
{
    double[,] new_matrix = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
    for (int i = 0; i < new_matrix.GetLength(0); i++)
    {
        for (int j = 0; j < new_matrix.GetLength(1); j++)
        {
            if (i >= index_row && j >= index_column)
                new_matrix[i, j] = matrix[i + 1, j + 1];
            else if (i >= index_row)
                new_matrix[i, j] = matrix[i + 1, j];
            else if (j >= index_row)
                new_matrix[i, j] = matrix[i, j + 1];
            else
                new_matrix[i, j] = matrix[i, j];
        }
    }
    return new_matrix;
}

double GetDeterminant(double[,] matrix)
{
    int size = matrix.GetLength(0);

    switch(size)
    {
        case 1: return matrix[0, 0];
        case 2: return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
        default:
            double determinant = 0;
            for (int column = 0; column < size; column++)
            {
                determinant += (column % 2 == 0 ? 1 : -1) * matrix[0, column] * GetDeterminant(GetMatrixWithout(matrix, 0, column));
            }
            return determinant;
    }
}

double[,] GetTranspose(double[,] matrix)
{
    double[,] transpose = new double[matrix.GetLength(1), matrix.GetLength(0)];
    for (int i = 0; i < transpose.GetLength(0); i++)
    {
        for (int j = 0; j < transpose.GetLength(1); j++)
        {
            transpose[i, j] = matrix[j, i];
        }
    }
    return transpose;
}

double[,] GetAlgebraicAddition(double[,] matrix)
{
    double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            double[,] without = GetMatrixWithout(matrix, i, j);
            result[i, j] = ((i + j) % 2 is 0 ? 1 : -1) * GetDeterminant(without);
        }
    }
    return result;
}

double[,] GetInverse(double[,] matrix)
{
    double det = GetDeterminant(matrix);
    double[,] algebraicadd = GetAlgebraicAddition(matrix);
    double[,] trans = GetTranspose(algebraicadd);
    double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
    for (int i = 0; i < result.GetLength(0); i++)
    {
        for (int j = 0; j < result.GetLength(1); j++)
        {
            result[i, j] = trans[i, j] / det;
        }
    }
    return result;
}

//double[,] Multiply(double[,] matrix, double number)
//{
//    double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
//    for (int i = 0; i < result.GetLength(0); i++)
//    {
//        for (int j = 0; j < result.GetLength(1); j++)
//        {
//            result[i, j] = matrix[i, j] * number;
//        }
//    }
//    return result;
//}
#endregion