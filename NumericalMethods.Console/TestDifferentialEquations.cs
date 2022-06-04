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

            ResultTable resultTwo = Calculate(
            function: "2*x",
            b: 10,
            h: 1,
            initialGuess: (x: 1, ys: new Dictionary<string, double>()
            {
                {"y0", 1},
            }));
            ResultTable resultThree = Calculate(
            function: "2*(x^2 + y0)",
            b: 1,
            h: 0.1,
            initialGuess: (x: 0, ys: new Dictionary<string, double>()
            {
                {"y0", 1},
            }));
            ResultTable resultFour = Calculate(
            function: "3*x^2*y0 + x^2*e^(x^3)",
            b: 1,
            h: 0.1,
            initialGuess: (x: 0, ys: new Dictionary<string, double>()
            {
                 {"y0", 0},
            }));
            ResultTable resultFive = Calculate(
            function: "x - y0",
            b: 0.1,
            h: 0.01,
            initialGuess: (x: 0, ys: new Dictionary<string, double>()
            {
                  {"y0", 1},
                  {"y1", 0}
            }));
            ResultTable resultSix = Calculate(
            function: "x - y0 - 0*y1",
            b: 0.1,
            h: 0.01,
            initialGuess: (x: 0, ys: new Dictionary<string, double>()
            {
                              {"y0", 1},
                              {"y1", 1},
                              {"y2", 1},
                              {"y3", 1},
                              {"y4", 1},
                              {"y5", 1}
            }));
        }
        private static double EulerMethod(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            return yi.value + h * function.Evaluate(conditions).RealValue;
        }
        private static double EulerMethodRecalculation(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint> conditionsCopy = conditions.ToDictionary(pair => pair.Key, pair => pair.Value);
            double functionValue = function.Evaluate(conditionsCopy).RealValue;
            double y_next_approximation = yi.value + h * functionValue;
            conditionsCopy["x"] = x + h;
            conditionsCopy[yi.name] = y_next_approximation;
            return yi.value + (h * (functionValue + function.Evaluate(conditionsCopy).RealValue)) / 2;
        }
        private static double EulerIterativeMethod(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint> conditionsCopy = conditions.ToDictionary(pair => pair.Key, pair => pair.Value);
            double functionValue = function.Evaluate(conditionsCopy).RealValue;
            double y_next = yi.value + h * functionValue;
            conditionsCopy["x"] = x + h;
            for (int k = 0; k < 4; k++)
            {
                conditionsCopy[yi.name] = y_next;
                y_next = yi.value + h * (functionValue + function.Evaluate(conditionsCopy).RealValue) / 2;
            }
            return y_next;
        }
        private static double EulerMethodImproved(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint> conditionsCopy = conditions.ToDictionary(pair => pair.Key, pair => pair.Value);
            double y_half = yi.value + h / 2 * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy["x"] = x + h / 2;
            conditionsCopy[yi.name] = y_half;
            return yi.value + h * function.Evaluate(conditionsCopy).RealValue;
        }
        static double RungeKuttaThridOrderMethods(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint> conditionsCopy = conditions.ToDictionary(pair => pair.Key, pair => pair.Value);

            double y_delta, k_one, k_two, k_three;

            k_one = h * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy["x"] = x + h / 3;
            conditionsCopy[yi.name] = yi.value + k_one / 3;
            k_two = h * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy["x"] = x + h * 2.0 / 3.0;
            conditionsCopy[yi.name] = yi.value + k_two * 2.0 / 3.0;
            k_three = h * function.Evaluate(conditionsCopy).RealValue;
            y_delta = (k_one + 3 * k_three) / 4;
            return yi.value + y_delta;
        }
        static double RungeKuttaFourthOrderMethods(SymbolicExpression function, double h, double x, (string name, double value) yi, Dictionary<string, FloatingPoint> conditions)
        {
            Dictionary<string, FloatingPoint> conditionsCopy = conditions.ToDictionary(pair => pair.Key, pair => pair.Value);
            double y_delta, k_one, k_two, k_three, k_four;
            k_one = h * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy["x"] = x + h / 2;
            conditionsCopy[yi.name] = yi.value + k_one / 2;
            k_two = h * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy[yi.name] = yi.value + k_two / 2;
            k_three = h * function.Evaluate(conditionsCopy).RealValue;
            conditionsCopy["x"] = x + h;
            conditionsCopy[yi.name] = yi.value + k_three;
            k_four = h * function.Evaluate(conditionsCopy).RealValue;
            y_delta = (k_one + 2 * k_two + 2 * k_three + k_four) / 6;
            return yi.value + y_delta;
        }
/*        static IEnumerable<(double x, double y)> AdamsMethod(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = RungeKuttaFourthOrderMethods(fun, a, b, h, conditions)
                .Take(4)
                .ToList();

            double y_next, x_current = value.Last().x;

            for (double i = x_current; i < b; i += h)
            {
                double delta_f, delta_two_f, delta_three_f;

                delta_f = function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y);

                delta_two_f = function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y) -
                   2 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y) +
                   function.EvaluateXY(value[value.Count - 3].x, value[value.Count - 3].y);

                delta_three_f = function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y) -
                   3 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y) +
                   3 * function.EvaluateXY(value[value.Count - 3].x, value[value.Count - 3].y) -
                   function.EvaluateXY(value[value.Count - 4].x, value[value.Count - 4].y);

                y_next = value[value.Count - 1].y + h * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y)
                    + Math.Pow(h, 2) * delta_f + 5 * Math.Pow(h, 3) / 12 * delta_two_f + 3 * Math.Pow(h, 4) / 8 * delta_three_f;

                x_current += h;

                value.Add((x_current, y_next));
            }

            return value;
        }
        static IEnumerable<(double x, double y)> AdamsBashforthMethod(string fun, double a, double b, double h, (double x, double y) conditions)
        {
            SymbolicExpression function = SymbolicExpression.Parse(fun);

            List<(double x, double y)> value = new List<(double, double)>()
            {
                (conditions.x, conditions.y)
            };

            double x_current = value[value.Count - 1].x, f1, f2, f3, f4, f5;

            for (double i = a; i < b; i += h)
            {
                f1 = value[value.Count - 1].y + h * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y);
                x_current += h;
                value.Add((x_current, f1));

                f2 = f1 + h * (3 / 2 * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y)
                    - 1 / 2 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y));
                x_current += h;
                value.Add((x_current, f2));

                f3 = f2 + h * (23 / 12 * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y)
                    - 4 / 3 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y)
                    + 5 / 12 * function.EvaluateXY(value[value.Count - 3].x, value[value.Count - 3].y));
                x_current += h;
                value.Add((x_current, f3));

                f4 = f3 + h * (55 / 24 * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y)
                    - 59 / 24 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y)
                    + 37 / 24 * function.EvaluateXY(value[value.Count - 3].x, value[value.Count - 3].y)
                    - 3 / 8 * function.EvaluateXY(value[value.Count - 4].x, value[value.Count - 4].y));
                x_current += h;
                value.Add((x_current, f4));

                f5 = f4 + h * (1901 / 720 * function.EvaluateXY(value[value.Count - 1].x, value[value.Count - 1].y)
                    - 1387 / 360 * function.EvaluateXY(value[value.Count - 2].x, value[value.Count - 2].y)
                    + 109 / 30 * function.EvaluateXY(value[value.Count - 3].x, value[value.Count - 3].y)
                    - 637 / 360 * function.EvaluateXY(value[value.Count - 4].x, value[value.Count - 4].y)
                    + 251 / 720 * function.EvaluateXY(value[value.Count - 5].x, value[value.Count - 5].y));
                x_current += h;
                value.Add((x_current, f5));

                i = x_current;
            }
            return value;
        }*/
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
                Dictionary<string, FloatingPoint> conditions = new Dictionary<string, FloatingPoint>() { { "x", current_x - h } };
                foreach (var yName in initialGuess.ys.Keys)
                {
                    conditions.Add(yName, result[yName].Last().yi);
                }
                foreach (KeyValuePair<string, double> y in initialGuess.ys)
                {
                    var newValueY = EulerIterativeMethod(SymbolicExpression.Parse(functions[y.Key]), h, current_x - h, (y.Key, conditions[y.Key].RealValue), conditions);
                    ys.Add(y.Key, newValueY);
                }
                result.Add(current_x, ys);
            }
            return result;
        }
    }
}
