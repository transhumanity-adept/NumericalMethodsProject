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
            ResultTable result = Calculate(
            function: "x - 2*y1 - y0",
            b: 1,
            h: 0.2,
            initialGuess: (x: 0, ys: new Dictionary<string, double>()
            {
                {"y0", 1},
                {"y1", 0}
            }));

            /*            System.Console.WriteLine("Метод Эйлера");
                        EulerMethod(func, a, b, h, conditions)
                            .ToList()
                            .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));*/

            /*            System.Console.WriteLine(new string('-', 100));

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
                            .ForEach(value => System.Console.WriteLine($"{value.Item1} - {value.Item2}"));*/
        }
        private static double EulerMethod(SymbolicExpression function, double h, double x, double y, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint>? conditionsWithX = conditions
                .Append(new KeyValuePair<string, FloatingPoint>("x", x))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            return y + h * function.Evaluate(conditionsWithX).RealValue;
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

        static double RungeKuttaFourthOrderMethods(SymbolicExpression function, double h, double x, double y, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint>? conditionsWithX = conditions
                .Append(new KeyValuePair<string, FloatingPoint>("x", x))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            double k_one, k_two, k_three, k_four, l_one, l_two, l_three, l_four, z;
            z = function.Evaluate(conditionsWithX).RealValue;
            k_one = y;
            l_one = z;
            k_two = z + h * l_one / 2;
            l_two = (-2 * (z + h * l_one / 2) - (y + h * k_one / 2) + x + h / 2);
            k_three = z + h * l_two / 2;
            l_three = (-2 * (z + h * l_two / 2) - (y + h * k_two / 2) + x + h / 2);
            k_four = z + h * l_three;
            l_four = (-2 * (z + h * l_three) - (y + h * k_three) + x + h);
            return y + h / 6 * (k_one + 2 * k_two + 2 * k_three + k_four);
        }

        private static ResultTable Calculate(string function, double b, double h, (double x, Dictionary<string, double> ys) initialGuess)
        {
            Dictionary<string, double> sortedYs = initialGuess.ys
                .OrderBy(pair => pair.Key)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            KeyValuePair<string, double> maxOrderY = sortedYs.Last();
            var order = int.Parse(maxOrderY.Key[1..]);
            Dictionary<string, string> functions = new Dictionary<string, string>();
            for (int i = 0; i < order; i++)
            {
                functions.Add(sortedYs.ElementAt(i).Key, sortedYs.ElementAt(i + 1).Key);
            }
            functions.Add(maxOrderY.Key, function);
            ResultTable result = new ResultTable(order);
            result.Add(initialGuess.x, initialGuess.ys);
            for (double current_x = initialGuess.x + h; Math.Round(current_x, 7) <= Math.Round(b, 7); current_x += h)
            {
                Dictionary<string, double> ys = new Dictionary<string, double>();
                Dictionary<string, FloatingPoint> conditions = new Dictionary<string, FloatingPoint>();
                foreach (var yName in initialGuess.ys.Keys)
                {
                    conditions.Add(yName, result[yName].Last().yi);
                }
                foreach (KeyValuePair<string, double> y in initialGuess.ys)
                {
                    var newValueY = RungeKuttaFourthOrderMethods(SymbolicExpression.Parse(functions[y.Key]), h, current_x - h, conditions[y.Key].RealValue, conditions);
                    ys.Add(y.Key, newValueY);
                }
                result.Add(current_x, ys);
            }
            return result;
        }
    }
}
