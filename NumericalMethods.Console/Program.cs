using org.mariuszgromada.math.mxparser;

Function function = new Function("f(x) = x^2");
double a = -5;
double b = 5;
int n = 3;

Console.WriteLine($"S = {GaussIntegration(function, a, b, n)}");

double GaussIntegration(Function function, double a, double b, int n)
{
    double[] acoef = new double[n];
    double[] x = new double[n];
    for (int i = 1; i <= n; i++)
    {
        double ti = T(n, i, n);
        double pd = PD(n, ti);
        double diva = (1 - ti * ti) * pd * pd;
        acoef[i - 1] = 2 / diva;
        x[i - 1] = GetRealX(a, b, ti);
    }

    return (b - a) / 2 * acoef.Zip(x, (ac, xc) => ac * function.calculate(xc)).Sum();
}

double GetRealX(double a, double b, double t)
{
    return (a + b) / 2 + (b - a) / 2 * t;
}

double T(int k, int i, int n)
{
    if(k == 0)
    {
        double t1 = Math.PI * (4 * i - 1);
        double t2 = 4 * n + 2;
        return Math.Cos(t1 / t2);
    }
    double tprev = T(k - 1, i, n);
    return tprev - P(n, tprev) / PD(n, tprev); 
}

double P(double k, double t)
{
    return k switch
    {
        0 => 1,
        1 => t,
        _ => (2 * k + 1) / (k + 1) * t * P(k - 1, t) 
             - k / (k + 1) * P(k - 2, t)
    };
}

double PD(double n, double t)
{
    double left = n / (1 - t * t);
    double right = P(n - 1, t) - t * P(n, t);
    return left * right;
}
