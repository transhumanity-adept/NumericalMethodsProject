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
using System.Data;
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
            InitializeComponent();


			Width = 1300;
			Height = 750;

			InitializeDifferentiation();
			InitializeIntegration();
			InitializeNonLinerEquation();

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
			if (String.IsNullOrEmpty(Differentiation_FunctionTextBox.Text) ||
				String.IsNullOrEmpty(Differentiation_StartXTextBox.Text) ||
				String.IsNullOrEmpty(Differentiation_EndXTextBox.Text) ||
				String.IsNullOrEmpty(Differentiation_StepTextBox.Text))
			{
				MessageBox.Show("Для начала рассчёта, заполните все поля");
				return;
			}
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
		private void Differentiation_FunctionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Differentiation_AccuracyGrid.Visibility = Visibility.Collapsed;
			Differentiation_NumberOfMembersGrid.Visibility = Visibility.Collapsed;
			Differentiation_Accuracy.Text = "";
			Differentiation_NumberOfMembers.Text = "";
			DifferentiationFunctionType differentiationFunctionType = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), ((ComboBox)sender).SelectedValue.ToString());
			if(differentiationFunctionType == DifferentiationFunctionType.NewtonPolynomials)
            {
				Differentiation_NumberOfMembersGrid.Visibility = Visibility.Visible;

			}
			if(differentiationFunctionType == DifferentiationFunctionType.Runge || differentiationFunctionType == DifferentiationFunctionType.UndefinedCoefficients)
            {
				Differentiation_AccuracyGrid.Visibility = Visibility.Visible;
			}
		}
		private void Differentiation_IntTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0)))
			{
				e.Handled = true;
			}
		}
		private void Differentiation_DoubleTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
			   && (!((TextBox)sender).Text.Contains(".")
			   && ((TextBox)sender).Text.Length != 0)))
			{
				e.Handled = true;
			}
		}
		private void Differentiation_AddOnChartInterpolationButton_Click(object sender, RoutedEventArgs e)
		{
			if(_points.Count == 0)
            {
				MessageBox.Show("Добавьте точки");
				return;
            }
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
			if (_points.Count == 0)
			{
				MessageBox.Show("Добавьте точки");
				return;
			}
			var xs = new List<double>();
			var ys = new List<double>();
			_start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
			_end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
			_step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();
			int roundNumbers = _step.ToString().Contains(',') ? _step.ToString().Split(',')[1].Length % 16 : 1;
			int derivative_degree = int.Parse(DerivativeDegreeTextBox.Text.Trim());
			string? function_type_string = Differentiation_FunctionTypeComboBox.SelectedValue.ToString();
			if (function_type_string is null) return;

			DifferentiationFunctionType interpolation_type = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), function_type_string);

            switch (interpolation_type)
            {
				case DifferentiationFunctionType.NewtonPolynomials:
                    if (String.IsNullOrEmpty(Differentiation_NumberOfMembers.Text))
                    {
						MessageBox.Show("Введите number of members");
						return;
                    }
					int numberOfMembers = int.Parse(Differentiation_NumberOfMembers.Text);
					INewtonDifferentiationFunction newton_function = DifferentiationBuilder.CreateNewton(_points, _step, derivative_degree, numberOfMembers);
					for (double x = _start_x; Math.Round(x, roundNumbers) <= _end_x; x += _step)
					{
                        double? y = newton_function.Calculate(x);
						if (y is null) continue;

						xs.Add(x);
						ys.Add((double)y);
					}
					break;
				case DifferentiationFunctionType.UndefinedCoefficients:
                    if (String.IsNullOrEmpty(Differentiation_Accuracy.Text))
                    {
						MessageBox.Show("Введите accuracy");
						return;
                    }
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
						if (String.IsNullOrEmpty(Differentiation_Accuracy.Text))
						{
							MessageBox.Show("Введите accuracy");
							return;
						}
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
			foreach(var item in Enum.GetValues(typeof(IntegrationMethodsWithVariableStep)))
            {
				Integration_MethodComboBox.Items.Add(item.ToString());
			}
			Integration_MethodComboBox.Items.Add("MonteCarlo");

			Integration_MethodComboBox.SelectedItem = Integration_MethodComboBox.Items[0];

            Integration_FunctionTextBox.Text = "x^2";
            Integration_StartXTextBox.Text = "-5";
            Integration_EndXTextBox.Text = "5";
            Integration_StepTextBox.Text = "0.1";

            Integration_MainChart.Plot.Legend(enable: true);
		}
		private void Integration_IntTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0)))
			{
				e.Handled = true;
			}
		}
		private void Integration_DoubleTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
			   && (!((TextBox)sender).Text.Contains(".")
			   && ((TextBox)sender).Text.Length != 0)))
			{
				e.Handled = true;
			}
		}
		private void Integration_MethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Enum.IsDefined(typeof(IntegrationMethodsWithConstantStep), ((ComboBox)sender).SelectedValue))
			{
				Integration_CountNodesGrid.Visibility = Visibility.Collapsed;
				Integration_CountPointGrid.Visibility = Visibility.Collapsed;
			}
			if (Enum.IsDefined(typeof(IntegrationMethodsWithVariableStep), ((ComboBox)sender).SelectedValue))
			{
				Integration_CountNodesGrid.Visibility = Visibility.Visible;
				Integration_CountPointGrid.Visibility = Visibility.Collapsed;
			}
			if (((ComboBox)sender).SelectedValue.ToString() == "MonteCarlo")
			{
				Integration_CountNodesGrid.Visibility = Visibility.Collapsed; 
				Integration_CountPointGrid.Visibility = Visibility.Visible;
			}
			Integration_CountPointTextBox.Text = "";
			Integration_CountNodesTextBox.Text = "";
		}
		private void Integration_CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			Integration_MainChart.Plot.Clear();
			if (String.IsNullOrEmpty(Integration_FunctionTextBox.Text)||
				String.IsNullOrEmpty(Integration_StartXTextBox.Text)||
				String.IsNullOrEmpty(Integration_EndXTextBox.Text)||
				String.IsNullOrEmpty(Integration_StepTextBox.Text))
            {
				MessageBox.Show("Для начала рассчёта, заполните все поля");
				return;
            }
			Function function = new Function("f(x) = " + Integration_FunctionTextBox.Text.Trim());
			double start_x = new Expression(Integration_StartXTextBox.Text.Trim()).calculate();
			double end_x = new Expression(Integration_EndXTextBox.Text.Trim()).calculate();
			double step = new Expression(Integration_StepTextBox.Text.Trim()).calculate();

			string function_type_string = Integration_MethodComboBox.SelectedValue.ToString();
			double integration_result = 0;
			if (Enum.IsDefined(typeof(IntegrationMethodsWithConstantStep), function_type_string))
			{
				IntegrationMethodsWithConstantStep method = (IntegrationMethodsWithConstantStep)Enum.Parse(typeof(IntegrationMethodsWithConstantStep), function_type_string);
				IIntegratorWithConstantStep integrator = new IntegrationBuilder().Build(Integration_FunctionTextBox.Text.Trim(), method);
				integration_result = integrator.Integrate(start_x, end_x, step);
			}
			if (Enum.IsDefined(typeof(IntegrationMethodsWithVariableStep), function_type_string))
			{
                if (String.IsNullOrEmpty(Integration_CountNodesTextBox.Text))
                {
					MessageBox.Show("Для начала рассчёта, заполните поле count nodes");
					return;
                }
				int countNodes = int.Parse(Integration_CountNodesTextBox.Text);
				IntegrationMethodsWithVariableStep method = (IntegrationMethodsWithVariableStep)Enum.Parse(typeof(IntegrationMethodsWithVariableStep), function_type_string);
				IIntegratorWithVariableStep integrator = new IntegrationBuilder().Build(Integration_FunctionTextBox.Text.Trim(), method);
				integration_result = integrator.Integrate(start_x, end_x, countNodes);
			}
			if (function_type_string == "MonteCarlo")
			{
				if (String.IsNullOrEmpty(Integration_CountPointTextBox.Text))
				{
					MessageBox.Show("Для начала рассчёта, заполните поле count point");
					return;
				}
				int countPoints = int.Parse(Integration_CountPointTextBox.Text);
				IIntegratorMonteCarloMethod integrator = new IntegrationBuilder().BuildMonteCarlo(Integration_FunctionTextBox.Text.Trim());
				integration_result = integrator.Integrate(start_x, end_x, countPoints);
			}

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
		#region SystemOfNonLinearEquationsTab
		private void InitializeNonLinerEquation()
		{
			Enum.GetNames(typeof(SolvingMethods)).ToList().ForEach(nameSolvingMethod =>
			{
				SNE_methodComboBox.Items.Add(nameSolvingMethod);
			});
			for (int i = 2; i <= 5; i++)
			{
				SNE_equationsCountComboBox.Items.Add(i.ToString());
			};
			SNE_methodComboBox.SelectedItem = SNE_methodComboBox.Items[0];
			SNE_equationsCountComboBox.SelectedItem = SNE_equationsCountComboBox.Items[0];
		}
		private void SNE_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0) || (e.Text == ",")
			   && (!((TextBox)sender).Text.Contains(",")
			   && ((TextBox)sender).Text.Length != 0)))
			{
				e.Handled = true;
			}
		}

		private void InitialDefaulSystemOfEquations(int equations_count)
		{
			SNE_systemOfEquationsGrid.Children.Clear();
			SNE_systemOfEquationsGrid.RowDefinitions.Clear();
			Canvas equationsCanvas;
			int start_position = 10;
			for (int i = 0; i < equations_count; i++)
			{
				RowDefinition rowDefinition = new RowDefinition();
				rowDefinition.Height = new GridLength(52, GridUnitType.Pixel);
				equationsCanvas = new Canvas();
				SNE_systemOfEquationsGrid.RowDefinitions.Add(rowDefinition);
				SNE_systemOfEquationsGrid.Children.Add(equationsCanvas);

				Label equationLabel = new Label();
				equationLabel.Content = "f(x) = ";
				equationLabel.FontSize = 14;
				equationLabel.Margin = new Thickness(10, start_position, 0, 0);
				equationsCanvas.Children.Add(equationLabel);

				TextBox equationOfSystem = new TextBox();
				equationOfSystem.Margin = new Thickness(60, start_position, 20, 0);
				equationOfSystem.FontSize = 10;
				equationOfSystem.Width = 150;
				equationOfSystem.Height = 30;
				equationsCanvas.Children.Add(equationOfSystem);
				start_position += 40;
			}
		}

		private void InitialOtherSystemOfEquations(int equations_count)
		{
			SNE_systemOfEquationsGrid.Children.Clear();
			SNE_systemOfEquationsGrid.RowDefinitions.Clear();
			Canvas equationsCanvas;
			int start_position = 10;
			for (int i = 1; i <= equations_count; i++)
			{
				RowDefinition rowDefinition = new RowDefinition();
				rowDefinition.Height = new GridLength(52, GridUnitType.Pixel);
				equationsCanvas = new Canvas();
				SNE_systemOfEquationsGrid.RowDefinitions.Add(rowDefinition);
				SNE_systemOfEquationsGrid.Children.Add(equationsCanvas);

				Label equationLabel = new Label();
				equationLabel.Content = $"x{i} = ";
				equationLabel.FontSize = 14;
				equationLabel.Margin = new Thickness(10, start_position, 0, 0);
				equationsCanvas.Children.Add(equationLabel);

				TextBox equationOfSystem = new TextBox();
				equationOfSystem.Margin = new Thickness(60, start_position, 20, 0);
				equationOfSystem.FontSize = 10;
				equationOfSystem.Width = 150;
				equationOfSystem.Height = 30;
				equationsCanvas.Children.Add(equationOfSystem);
				start_position += 40;
			}
		}

		private void GenerateInitialApproximation(int equations_count)
		{
			SNE_initialApproximationGrid.Children.Clear();
			SNE_initialApproximationGrid.RowDefinitions.Clear();
			Canvas initialApproximationCanvas;
			int start_position = 10;
			for (int i = 1; i <= equations_count; i++)
			{
				RowDefinition rowDefinition = new RowDefinition();
				rowDefinition.Height = new GridLength(52, GridUnitType.Pixel);
				initialApproximationCanvas = new Canvas();
				SNE_initialApproximationGrid.RowDefinitions.Add(rowDefinition);
				SNE_initialApproximationGrid.Children.Add(initialApproximationCanvas);

				Label equationLabel = new Label();
				equationLabel.Content = $"x{i}";
				equationLabel.FontSize = 12;
				equationLabel.Margin = new Thickness(10, start_position, 0, 0);
				initialApproximationCanvas.Children.Add(equationLabel);

				TextBox equationOfSystem = new TextBox();
				equationOfSystem.Margin = new Thickness(60, start_position, 20, 0);
				equationOfSystem.FontSize = 10;
				equationOfSystem.Width = 50;
				equationOfSystem.Height = 30;
				equationOfSystem.PreviewTextInput += SNE_textBox_PreviewTextInput;
				initialApproximationCanvas.Children.Add(equationOfSystem);
				start_position += 40;
			}
		}

		private void GenerateResultTable(IEnumerable<IEnumerable<double>> result, IEnumerable<string> variablesNames, SolvingMethods solvingMethod)
		{
			variablesNames = variablesNames.OrderBy(el => el);
			DataTable dataTable = new DataTable();
			SNE_ResultDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
			dataTable.Columns.Add("k");
			foreach (var variableName in variablesNames)
			{
				dataTable.Columns.Add($"{variableName}(k)");
			}
			dataTable.Columns.Add(solvingMethod == SolvingMethods.Secant ? "Sk" : "Difference");
			int countResult = result.Count();
			for (int i = 0; i < countResult; i++)
			{
				object[] row = new object[variablesNames.Count() + 2];
				row[0] = i;
				for (int j = 0; j < result.ElementAt(i).Count() - 1; j++)
				{
					row[j + 1] = string.Format("{0:F4}",result.ElementAt(i).ElementAt(j));
				}
				row[row.Length - 1] = result.ElementAt(i).Last() == double.MinValue ? "-" : string.Format("{0:F4}", result.ElementAt(i).Last());
				dataTable.Rows.Add(row);
			}
			SNE_ResultDataGrid.ItemsSource = dataTable.AsDataView();
		}

		private void SNE_SelectButton_Click(object sender, RoutedEventArgs e)
		{
			SNE_ResultDataGrid.ItemsSource = null;
			int equations_count = int.Parse(SNE_equationsCountComboBox.SelectedItem.ToString());
			string method_type = SNE_methodComboBox.SelectedItem.ToString();
			if (method_type == SolvingMethods.Seidel.ToString() || method_type == SolvingMethods.SimpleIterations.ToString()) { InitialOtherSystemOfEquations(equations_count); }
			else { InitialDefaulSystemOfEquations(equations_count); }
			GenerateInitialApproximation(equations_count);

		}
		private void SNE_CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			if(SNE_systemOfEquationsGrid.Children.Count == 0 || SNE_initialApproximationGrid.Children.Count == 0)
            {
				MessageBox.Show("Добавьте уравнения и начальное приближение");
				return;
            }
			List<string> functions = new List<string>();
			foreach (var childrenGrid in SNE_systemOfEquationsGrid.Children)
			{
				Canvas canvas = childrenGrid as Canvas;
				foreach (var childrenCanvas in canvas.Children)
				{
					if (childrenCanvas is TextBox)
					{
						TextBox textBox = childrenCanvas as TextBox;
						if (String.IsNullOrEmpty(textBox.Text))
						{
							MessageBox.Show("Заполните все поля, связанные с функциями");
							return;
						}
						functions.Add(textBox.Text.Trim());
					}
				}
			}
			Dictionary<string, MathNet.Symbolics.FloatingPoint> initialGuess = new Dictionary<string, MathNet.Symbolics.FloatingPoint>();
			string initialGuessKey = null;
			MathNet.Symbolics.FloatingPoint initialGuessValue = null;
			foreach (var childrenGrid in SNE_initialApproximationGrid.Children)
			{
				Canvas canvas = childrenGrid as Canvas;
				foreach (var childrenCanvas in canvas.Children)
				{
					if (childrenCanvas is Label)
					{
						Label label = childrenCanvas as Label;
						initialGuessKey = label.Content.ToString();
					}
					if (childrenCanvas is TextBox)
					{
						TextBox textBox = childrenCanvas as TextBox;
						if (String.IsNullOrEmpty(textBox.Text))
						{
							MessageBox.Show("Заполните все поля, связанные с начальным приближением");
							return;
						}
						initialGuessValue = double.Parse(textBox.Text);
					}
				}
				initialGuess.Add(initialGuessKey, initialGuessValue);
			}
			if (String.IsNullOrEmpty(SNE_AccuracyTextBox.Text))
			{
				MessageBox.Show("Заполните поле точность");
				return;
			}
			IEnumerable<IEnumerable<double>> res;
			try
			{
				res = new NonLinearEquationsSolverBuilder().Build((SolvingMethods)Enum.Parse(typeof(SolvingMethods), SNE_methodComboBox.SelectedValue.ToString())).SolveWithSteps(new NonLinearEquationsSystem(functions), double.Parse(SNE_AccuracyTextBox.Text), initialGuess);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			res = res.Select(row => row.Select(item => Math.Round(item, 4)));
			GenerateResultTable(res, initialGuess.Select(el => el.Key), (SolvingMethods)Enum.Parse(typeof(SolvingMethods), SNE_methodComboBox.SelectedValue.ToString()));
		}

        #endregion

        private void DifferentiationEquation_ClearChartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DifferentiationEquation_AddOnChartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DifferentiationEquation_AddOnChartInterpolationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
