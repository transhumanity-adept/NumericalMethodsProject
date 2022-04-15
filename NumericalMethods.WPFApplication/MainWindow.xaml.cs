using NumericalMethods.Core.Approximation;
using NumericalMethods.Core.Approximation.Interfaces;
using NumericalMethods.Core.Differentiations;
using NumericalMethods.Core.Differentiations.Interfaces;
using NumericalMethods.Core.Integration;
using NumericalMethods.Core.Integration.Interfaces;
using NumericalMethods.WPFApplication.Integration;
using NumericalMethods.WPFApplication.Differentiation;

using org.mariuszgromada.math.mxparser;

using ScottPlot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Expression = org.mariuszgromada.math.mxparser.Expression;
using Point = NumericalMethods.WPFApplication.Differentiation.Point;

namespace NumericalMethods.WPFApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> _points = new List<Point>();
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

            Differentiation_FunctionTextBox.Text = "sin(x)";
            Differentiation_StartXTextBox.Text = "-4 * pi";
            Differentiation_EndXTextBox.Text = "4 * pi";
            Differentiation_StepTextBox.Text = "pi / 4";

            Differentiation_MainChart.Plot.Legend(enable: true);
        }

        private void Differentiation_AddNodesOnChart_Click(object sender, RoutedEventArgs e)
        {
            _points.Clear();
            Function function = new Function("f(x) = " + Differentiation_FunctionTextBox.Text.Trim());
            double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
            double step = new Expression(Differentiation_StepTextBox.Text.Trim()).calculate();

            for (double x = start_x; x <= end_x; x += step)
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
            string function_type_string = Differentiation_InterpolationFunctionTypeComboBox.SelectedValue.ToString();
            InterpolationFunctionType interpolation_type = (InterpolationFunctionType)Enum.Parse(typeof(InterpolationFunctionType), function_type_string);
            IInterpolationFunction? interpolation_function = InterpolationBuilder.Build(_points, interpolation_type);
            if (interpolation_function is null) return;
            var xs = new List<double>();
            var ys = new List<double>();
            double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
            for (double x = start_x; x <= end_x; x += 0.05)
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
            string function_type_string = Differentiation_FunctionTypeComboBox.SelectedValue.ToString();
            DifferentiationFunctionType interpolation_type = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), function_type_string);
            IDifferentiationFunction? interpolation_function = DifferentiationBuilder.Build(_points, interpolation_type, 0.05);
            if (interpolation_function is null) return;
            var xs = new List<double>();
            var ys = new List<double>();
            double start_x = new Expression(Differentiation_StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(Differentiation_EndXTextBox.Text.Trim()).calculate();
            for (double x = start_x; x <= end_x; x += 0.05)
            {
                int derivative_degree = int.Parse(DerivativeDegreeTextBox.Text.Trim());
                var y = interpolation_function.Calculate(x, derivative_degree);
                if (y is null) continue;

                xs.Add(x);
                ys.Add((double)y);
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

            (double[] xs, double[] ys) =  DataGenerate(function, start_x - 5, end_x + 5, step);

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
            IntegrationMethodsWithConstantStep method = (IntegrationMethodsWithConstantStep)Enum.Parse(typeof(IntegrationMethodsWithConstantStep), function_type_string);
            IIntegratorWithConstantStep integrator = new IntegrationBuilder()
                                                            .Build(new Integrand(function), method);
            double integration_result = integrator.Integrate(start_x, end_x, step);
            MessageBox.Show($"Integration result: {integration_result}");
        }
        #endregion

        #region Tools
        private static double[] DataGenerate(Function function, double start, double end, double step, FunctionAxis axis)
        {
            int size = (int)Math.Ceiling((end - start) / step);
            double[] result = new double[size + 1];
            int i = 0;
            switch (axis)
            {
                case FunctionAxis.Xs:
                    for (double x = start; x <= end; x += step, i++)
                    {
                        result[i] = x;
                    }
                    
                    break;
                case FunctionAxis.Ys:
                    for (double x = start; x <= end; x += step, i++)
                    {
                        result[i] = function.calculate(x);
                    }

                    break;
            }

            return result;
        }

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
    }
}
