using NumericalMethods.Core.Approximation;
using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiation.Interfaces;
using NumericalMethods.Core.Differentiations;
using NumericalMethods.Core.Differentiations.Interfaces;
using NumericalMethods.Infrastructure.Integration;
using NumericalMethods.Infrastructure.Integration.Interfaces;

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
		public MainWindow()
		{
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
			Function function = new Function("f(x) = " + Differentiation_FunctionTextBox.Text.Trim());
			double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = step.ToString().Contains(',') ? step.ToString().Split(',')[1].Length : 1;
			for (double x = start_x; Math.Round(x, roundNumbers) <= end_x; x += step)
			{
				double y = function.calculate(x);
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
			int roundNumbers = step.ToString().Contains(',') ? step.ToString().Split(',')[1].Length : 1;
			for (double x = start_x; Math.Round(x, roundNumbers) <= end_x; x += step)
			{
				var y = interpolation_function.Calculate(x);
				if (y is null) continue;

				xs.Add(x);
				ys.Add((double)y);
			}

			Differentiation_MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "interpolation");
			Differentiation_MainChart.Refresh();
		}

		private void Differentiation_AddOnChartButton_Click(object sender, RoutedEventArgs e)
		{
			var xs = new List<double>();
			var ys = new List<double>();
			double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = step.ToString().Contains(',') ? step.ToString().Split(',')[1].Length : 1;
			double numberOfMembers = new Expression(Differentiation_NumberOfMembers.Text.Trim()).calculate();
			int derivative_degree = int.Parse(DerivativeDegreeTextBox.Text.Trim());
			string? function_type_string = Differentiation_FunctionTypeComboBox.SelectedValue.ToString();
			if (function_type_string is null) return;
			DifferentiationFunctionType interpolation_type = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), function_type_string);
			INewtonDifferentiationFunction? newton_function = null;
			IDifferentiationFunction? differentiation_function = null;
			if (interpolation_type == DifferentiationFunctionType.NewtonPolynomials)
			{
				newton_function = DifferentiationBuilder.CreateNewton(_points, step, derivative_degree, (int)numberOfMembers);
			} else
			{
				differentiation_function = DifferentiationBuilder.Build(_points, interpolation_type, step);
			}
			if (differentiation_function is null && newton_function is null) return;
			if (Differentiation_Animate.IsChecked.Value)
			{
				Task.Run(async () =>
				{
					double last_x = start_x;
					double? last_y = newton_function is not null ? newton_function.Calculate(last_x)
							: differentiation_function.Calculate(last_x, derivative_degree);
					Color line_color = App.Current.Dispatcher.Invoke(() => Differentiation_MainChart.Plot.GetNextColor());
					for (double new_x = start_x + step; Math.Round(new_x, roundNumbers) <= end_x; new_x += step)
					{
						double? new_y = newton_function is not null ? newton_function.Calculate(new_x)
							: differentiation_function.Calculate(new_x, derivative_degree);
						if (new_y is null || last_y is null) continue;
						App.Current.Dispatcher.Invoke(() => Differentiation_MainChart.Plot.AddLine(last_x, last_y.Value, new_x, new_y.Value, line_color, lineWidth: 4));

						last_x = new_x;
						last_y = new_y;
						App.Current.Dispatcher.Invoke(() => Differentiation_MainChart.Refresh());
						await Task.Delay(1);
					}
				});
			} else
			{
				for (double x = start_x; Math.Round(x, roundNumbers) <= end_x; x += step)
				{
					var y = newton_function is not null ? newton_function.Calculate(x)
						: differentiation_function.Calculate(x, derivative_degree);
					if (y is null) continue;

					xs.Add(x);
					ys.Add((double)y);
				}
				Differentiation_MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "differentiation");
				Differentiation_MainChart.Refresh();
			}
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
