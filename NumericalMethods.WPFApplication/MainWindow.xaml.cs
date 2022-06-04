using NumericalMethods.Core.Approximation;
using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiation;
using NumericalMethods.Core.Differentiation.DifferentiationFunctions.UndefinedCoefficients;
using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Infrastructure.Integration;
using NumericalMethods.Infrastructure.Integration.Interfaces;
using NumericalMethods.Infrastructure.NonLinearEquationsSystems;
using NumericalMethods.Infrastructure.NonLinearEquationsSystems.Methods;

using org.mariuszgromada.math.mxparser;

using ScottPlot;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Expression = org.mariuszgromada.math.mxparser.Expression;
using Point = NumericalMethods.WPFApplication.Differentiation.Point;

namespace NumericalMethods.WPFApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		private readonly List<Point> _points = new List<Point>();
		private Function _current_function;
		private double _start_x;
		private double _end_x;
		private double _step;
		public MainWindow()
		{
            //IEnumerable<IEnumerable<double>> res = new NonLinearEquationsSolverBuilder().Build(SolvingMethods.Secant).SolveWithSteps(new NonLinearEquationsSystem(new List<string>()
            //{
            //    "x1 + x2 - 3",
            //    "x1^2 + x2^2 - 9"
            //}), 0.001, new Dictionary<string, MathNet.Symbolics.FloatingPoint>() { { "x2", 5.0 }, { "x1", 1.0 } });
            //IEnumerable<IEnumerable<double>> res1 = new NonLinearEquationsSolverBuilder().Build(SolvingMethods.ModifiedNewton).SolveWithSteps(new NonLinearEquationsSystem(new List<string>()
            //{
            //	"x1 + x2 - 3",
            //	"x1^2 + x2^2 - 9"
            //}), 0.001, new Dictionary<string, MathNet.Symbolics.FloatingPoint>() { { "x2", 5.0 }, { "x1", 1.0 } });
            IEnumerable<IEnumerable<double>> res = new NonLinearEquationsSolverBuilder().Build(SolvingMethods.SimpleIterations).SolveWithSteps(new NonLinearEquationsSystem(new List<string>()
            {
                "sqrt((x1*(x2+5)-1)/2)",
                "sqrt(x1 + 3* lg(x1))"
            }), 0.001, new Dictionary<string, MathNet.Symbolics.FloatingPoint>() { { "x2", 2.2 }, { "x1", 3.5 } });


            InitializeComponent();


			Width = 1300;
			Height = 750;

			InitializeDifferentiation();
			InitializeIntegration();

		}

		#region DifferentiationTab
		private void InitializeDifferentiation()
		{
			foreach (var item in Enum.GetValues(typeof(InterpolationFunctionType)))
			{
				Differentiation_InterpolationFunctionTypeComboBox.Items.Add(item.ToString());
			}

			Differentiation_InterpolationFunctionTypeComboBox.SelectedItem = Differentiation_InterpolationFunctionTypeComboBox.Items[0];

			foreach (var item in Enum.GetValues(typeof(DifferentiationFunctionType)))
			{
				Differentiation_FunctionTypeComboBox.Items.Add(item.ToString());
			}

			Differentiation_FunctionTypeComboBox.SelectedItem = Differentiation_InterpolationFunctionTypeComboBox.Items[0];

			Differentiation_FunctionTextBox.Text = "x^2";
			Differentiation_StartXTextBox.Text = "-3";
			Differentiation_EndXTextBox.Text = "3";
			Differentiation_StepTextBox.Text = "1";
			Differentiation_NumberOfMembers.Text = "2";
			DerivativeDegreeTextBox.Text = "1";

			Differentiation_MainChart.Plot.Legend(enable: true);
		}

		private void Differentiation_AddNodesOnChart_Click(object sender, RoutedEventArgs e)
		{
			_points.Clear();
			_current_function = new Function("f(x) = " + Differentiation_FunctionTextBox.Text.Trim());
			double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = step.ToString().Contains(',') ? step.ToString().Split(',')[1].Length % 16 : 1;
			for (double x = start_x; Math.Round(x, roundNumbers) <= end_x; x += step)
			{
				double y = _current_function.calculate(x);
				_points.Add(new Point(x, y));
			}

			double[] xs = _points.Select(node => node.X).ToArray();
			double[] ys = _points.Select(node => node.Y).ToArray();
			Differentiation_MainChart.Plot.AddScatter(xs, ys, lineWidth: 0, markerShape: MarkerShape.filledCircle, color: System.Drawing.Color.Red, markerSize: 7, label: "nodes");
			Differentiation_MainChart.Refresh();
		}

		private void Differentiation_AddOnChartInterpolationButton_Click(object sender, RoutedEventArgs e)
		{
			string? function_type_string = Differentiation_InterpolationFunctionTypeComboBox.SelectedValue.ToString();
			if (function_type_string is null) return;
			InterpolationFunctionType interpolation_type = (InterpolationFunctionType)Enum.Parse(typeof(InterpolationFunctionType), function_type_string);
			IInterpolationFunction? interpolation_function = InterpolationBuilder.Build(_points, interpolation_type);
			if (interpolation_function is null) return;
			var xs = new List<double>();
			var ys = new List<double>();
			double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = step.ToString().Contains(',') ? step.ToString().Split(',')[1].Length % 16 : 1;
			for (double x = start_x; Math.Round(x, roundNumbers) <= end_x; x += step)
			{
				var y = interpolation_function.Calculate(x);
				if (y is null) continue;

				xs.Add(x);
				ys.Add((double)y);
			}

			Differentiation_MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "interpolation");
			Differentiation_MainChart.Refresh();
			Differentiation_MainChart.Refresh();
		}

		private void Differentiation_AddOnChartButton_Click(object sender, RoutedEventArgs e)
		{
			var xs = new List<double>();
			var ys = new List<double>();
			_start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			_end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			_step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = _step.ToString().Contains(',') ? _step.ToString().Split(',')[1].Length % 16 : 1;
			double numberOfMembers = new Expression(Differentiation_NumberOfMembers.Text.Trim()).calculate();
			int derivative_degree = int.Parse(DerivativeDegreeTextBox.Text.Trim());
			string? function_type_string = Differentiation_FunctionTypeComboBox.SelectedValue.ToString();
			if (function_type_string is null) return;

			DifferentiationFunctionType interpolation_type = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), function_type_string);

            switch (interpolation_type)
            {
				case DifferentiationFunctionType.NewtonPolynomials:
					INewtonDifferentiationFunction newton_function = DifferentiationBuilder.CreateNewton(_points, _step, derivative_degree, (int)numberOfMembers);
					for (double x = _start_x; Math.Round(x, roundNumbers) <= _end_x; x += _step)
					{
                        double? y = newton_function.Calculate(x);
						if (y is null) continue;

						xs.Add(x);
						ys.Add((double)y);
					}
					break;
				case DifferentiationFunctionType.UndefinedCoefficients:
					int count_coefficients_c = int.Parse(Differentiation_Accuracy.Text);
					List<Point> additional_right_points = new List<Point>();
                    for (int i = 1; i <= (count_coefficients_c - 2) * derivative_degree; i++)
                    {
						double new_x = _end_x + _step * i;
						double new_y = _current_function.calculate(new_x);
						Point additional_point = new Point(new_x, new_y);
						additional_right_points.Add(additional_point);
                    }

					List<Point> additional_left_point = new List<Point>();
                    for (int i = derivative_degree; i >= 1; i--)
                    {
						double new_x = _start_x - _step * i;
						double new_y = _current_function.calculate(new_x);
						Point additional_point = new Point(new_x, new_y);
						additional_left_point.Add(additional_point);
					}
					IEnumerable<Point> new_points = additional_left_point.Concat(_points.Concat(additional_right_points));
					IUndefinedCoefficientsDifferentiationFunction undefined_coefficients_function = DifferentiationBuilder.CreateUndefinedCoefficients(new_points, _step, derivative_degree, count_coefficients_c);
                    IEnumerable<IDifferentiationResultNode> result = undefined_coefficients_function.Calculate();
					xs = result.Select(node => node.X).ToList();
					ys = result.Select(node => node.Y).ToList();
					break;
				case DifferentiationFunctionType.Runge:
                    {
						IDifferentiationFunction differentiation_function = DifferentiationBuilder.CreateRunge(_points, _step, derivative_degree, 1, 2);
						for (double x = _start_x; Math.Round(x, roundNumbers) <= _end_x; x += _step)
						{
							double? y = differentiation_function.Calculate(x);
							if (y is null) continue;

							xs.Add(x);
							ys.Add((double)y);
						}
						break;
					}
				default:
                    {
						IDifferentiationFunction differentiation_function = DifferentiationBuilder.Build(_points, interpolation_type, _step, derivative_degree);
						for (double x = _start_x; Math.Round(x, roundNumbers) <= _end_x; x += _step)
						{
							double? y = differentiation_function.Calculate(x);
							if (y is null) continue;

							xs.Add(x);
							ys.Add((double)y);
						}
						break;
					}
			}
			
			Differentiation_MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "differentiation");
			Differentiation_MainChart.Refresh();
		}

		private void Differentiation_ClearChartButton_Click(object sender, RoutedEventArgs e)
		{
			Differentiation_MainChart.Plot.Clear();
			Differentiation_MainChart.Refresh();
		}
		#endregion

		#region IntegrationTab
		private void InitializeIntegration()
		{
			foreach (var item in Enum.GetValues(typeof(IntegrationMethodsWithConstantStep)))
			{
				Integration_MethodComboBox.Items.Add(item.ToString());
			}

			Integration_MethodComboBox.SelectedItem = Integration_MethodComboBox.Items[0];

			Integration_FunctionTextBox.Text = "x^2";
			Integration_StartXTextBox.Text = "-5";
			Integration_EndXTextBox.Text = "5";
			Integration_StepTextBox.Text = "0.1";

			Integration_MainChart.Plot.Legend(enable: true);
		}

		private void Integration_CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			Function function = new Function("f(x) = " + Integration_FunctionTextBox.Text.Trim());
			double start_x = new Expression(Integration_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Integration_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Integration_StepTextBox.Text.Trim()).calculate();

			(double[] xs, double[] ys) = DataGenerate(function, start_x - 5, end_x + 5, step);

			Integration_MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "function");

			double[] interval_start_x = { start_x, start_x };
			double[] interval_start_y = { 0, function.calculate(start_x) };
			Integration_MainChart.Plot.AddScatter(interval_start_x, interval_start_y, lineWidth: 4, markerSize: 0, label: "start integrate");

			double[] interval_end_x = { end_x, end_x };
			double[] interval_end_y = { 0, function.calculate(end_x) };
			Integration_MainChart.Plot.AddScatter(interval_end_x, interval_end_y, lineWidth: 4, markerSize: 0, label: "end integrate");

			(double[] fill_xs, double[] fill_ys) = DataGenerate(function, start_x, end_x, step);

			Integration_MainChart.Plot.AddFill(fill_xs, fill_ys, baseline: 0, color: System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red));
			Integration_MainChart.Plot.AddHorizontalLine(0, color: System.Drawing.Color.Black);
			Integration_MainChart.Refresh();

			string function_type_string = Integration_MethodComboBox.SelectedValue.ToString();
			//IntegrationMethodsWithConstantStep method = (IntegrationMethodsWithConstantStep)Enum.Parse(typeof(IntegrationMethodsWithConstantStep), function_type_string);
			//IIntegratorWithConstantStep integrator = new IntegrationBuilder()
			//																.Build(Integration_FunctionTextBox.Text.Trim(), method);
			IIntegratorWithVariableStep integrator = new IntegrationBuilder().Build(Integration_FunctionTextBox.Text.Trim(), IntegrationMethodsWithVariableStep.Chebyshev);
			double integration_result = integrator.Integrate(start_x, end_x, (int)step);
			MessageBox.Show($"Integration result: {integration_result}");
		}
		#endregion

		#region Tools
		private static (double[] xs, double[] ys) DataGenerate(Function function, double start, double end, double step)
		{
			int size = (int)Math.Ceiling((end - start) / step);
			double[] xs = new double[size + 1];
			double[] ys = new double[size + 1];
			int i = 0;
			for (double x = start; x <= end; x += step, i++)
			{
				xs[i] = x;
				ys[i] = function.calculate(x);
			}

			return (xs, ys);
		}
        #endregion

        private void textBoxLinearEquations_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.Key == Key.Enter)
            {
				RowDefinition rowDefinition = new RowDefinition();
				TextBox tbx1 = new TextBox();
				tbx1.Text = "fad";
				gridSystemLinearEquations.RowDefinitions.Add(rowDefinition);
				Grid.SetRow(tbx1, gridSystemLinearEquations.RowDefinitions.Count - 1);
				gridSystemLinearEquations.Children.Add(tbx1);
                tbx1.KeyDown += textBoxLinearEquations_KeyDown;
			}
        }
    }
}
