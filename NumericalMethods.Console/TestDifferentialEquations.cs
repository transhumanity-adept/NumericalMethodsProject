using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;

namespace NumericalMethods.Console
{
    public static class TestDifferentialEquations
    {
        public static void Run()
        {
            string func = "2*(x^2+y)";
            double a = 0, b = 1, h = 0.1;
            (double, double) conditions = (0, 1);

            System.Console.WriteLine("Метод Эйлера");
            EulerMethod(func, a, b, h, conditions)
                .ToList()
                .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));

            System.Console.WriteLine(new string('-', 100));

            System.Console.WriteLine("Метод Эйлера с перерасчётом");
            EulerMethodRecalculation(func, a, b, h, conditions)
                .ToList()
                .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));

            System.Console.WriteLine(new string('-', 100));

            System.Console.WriteLine("Метод Эйлера с итерационной обработкой");
            EulerIterativeMethod(func, a, b, h, conditions)
                .ToList()
                .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));

            System.Console.WriteLine(new string('-', 100));

            System.Console.WriteLine("Усовершенствованный метод Эйлера");
            EulerMethodImproved(func, a, b, h, conditions)
                .ToList()
                .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));

            System.Console.WriteLine(new string('-', 100));

            System.Console.WriteLine("Рунге-Кута метод");
            RungeKuttaMethods(func, a, b, h, conditions)
                .ToList()
                .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));
        }
        private static IEnumerable<(double x, double y)> EulerMethod(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>(){ (a, conditions.y) };

            double y_next, y_current, x_current;

            for (double i = a; i <= b; i += h)
            {
                y_current = value[value.Count - 1].y;
                x_current = value[value.Count - 1].x;
                y_next = y_current + h * (function.EvaluateXY(x_current, y_current));
                x_current += h;
                value.Add((x_current, y_next));
            }

            return value;
        }

        private static IEnumerable<(double x, double y)> EulerMethodRecalculation(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>()
    {
        (a, conditions.y)
    };

            double y_next_approximation, y_next, y_current, x_current;

            for (double i = a; i <= b; i += h)
            {
                y_current = value[value.Count - 1].y;
                x_current = value[value.Count - 1].x;
                y_next_approximation = y_current + h * (function.EvaluateXY(x_current, y_current));
                y_next = y_current + (h * (function.EvaluateXY(x_current, y_current) + function.EvaluateXY(x_current + h, y_next_approximation))) / 2;
                x_current += h;
                value.Add((x_current, y_next));
            }

            return value;

        }

        private static IEnumerable<(double x, double y)> EulerIterativeMethod(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>()
    {
        (a, conditions.y)
    };

            double y_next, y_current, x_current;

            for (double i = a; i <= b; i += h)
            {
                y_current = value[value.Count - 1].y;
                x_current = value[value.Count - 1].x;

                y_next = y_current + h * (function.EvaluateXY(x_current, y_current));

                for (int k = 0; k < 4; k++)
                {
                    y_next = y_current + h * (function.EvaluateXY(x_current, y_current) + function.EvaluateXY(x_current + h, y_next)) / 2;
                }

                x_current += h;
                value.Add((x_current, y_next));
            }
            return value;
        }

        private static IEnumerable<(double x, double y)> EulerMethodImproved(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>()
    {
        (a, conditions.y)
    };

            double y_next, y_half, y_current, x_current;

            for (double i = a; i <= b; i += h)
            {
                y_current = value[value.Count - 1].y;
                x_current = value[value.Count - 1].x;

                y_half = y_current + h / 2 * (function.EvaluateXY(x_current, y_current));
                y_next = y_current + h * (function.EvaluateXY(x_current + h / 2, y_half));
                x_current += h;
                value.Add((x_current, y_next));
            }

            return value;
        }


        // Метод из учебника Турчак Л.И., Плотников П.В. - основы численных методов
        private static IEnumerable<(double x, double y)> RungeKuttaMethods(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>()
    {
        (a, conditions.y)
    };

            double y_next, y_current, x_current, y_delta, k_one, k_two, k_three, k_four;

            for (double i = a; i <= b; i += h)
            {
                y_current = value[value.Count - 1].y;
                x_current = value[value.Count - 1].x;

                k_one = function.EvaluateXY(x_current, y_current);
                k_two = function.EvaluateXY(x_current + h / 2, y_current + (h * k_one) / 2);
                k_three = function.EvaluateXY(x_current + h / 2, y_current + (h * k_two) / 2);
                k_four = function.EvaluateXY(x_current + h, y_current + (h * k_three));
                y_delta = h / 6 * (k_one + 2 * k_two + 2 * k_three + k_four);
                y_next = y_current + y_delta;
                x_current += h;
                value.Add((x_current, y_next));
            }

            return value;
        }
    }
}
