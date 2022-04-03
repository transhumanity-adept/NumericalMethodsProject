using NumericalMethods.Approximation.Interpolations;
using NumericalMethods.Approximation.Interpolations.Interfaces;
using NumericalMethods.Differentiations;
using NumericalMethods.Differentiations.Interfaces;

using org.mariuszgromada.math.mxparser;

using ScottPlot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Expression = org.mariuszgromada.math.mxparser.Expression;

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

            foreach (var item in Enum.GetValues(typeof(InterpolationFunctionType)))
            {
                InterpolationFunctionTypeComboBox.Items.Add(item.ToString());
            }

            InterpolationFunctionTypeComboBox.SelectedItem = InterpolationFunctionTypeComboBox.Items[0];

            foreach (var item in Enum.GetValues(typeof(DifferentiationFunctionType)))
            {
                DifferentiationFunctionTypeComboBox.Items.Add(item.ToString());
            }

            DifferentiationFunctionTypeComboBox.SelectedItem = InterpolationFunctionTypeComboBox.Items[0];

            Width = 1300;
            Height = 750;

            FunctionTextBox.Text = "sin(x)";
            StartXTextBox.Text = "-4 * pi";
            EndXTextBox.Text = "4 * pi";
            StepTextBox.Text = "pi / 4";

            MainChart.Plot.Legend(enable: true);
        }

        private void AddNodesOnChart_Click(object sender, RoutedEventArgs e)
        {
            _points.Clear();
            Function function = new Function("f(x) = " + FunctionTextBox.Text.Trim());
            double start_x = new Expression(StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(EndXTextBox.Text.Trim()).calculate();
            double step = new Expression(StepTextBox.Text.Trim()).calculate();

            for (double x = start_x; x <= end_x; x += step)
            {
                double y = function.calculate(x);
                _points.Add(new Point(x, y));
            }

            double[] xs = _points.Select(node => node.X).ToArray();
            double[] ys = _points.Select(node => node.Y).ToArray();
            MainChart.Plot.AddScatter(xs, ys, lineWidth: 0, markerShape: MarkerShape.filledCircle, color: System.Drawing.Color.Red, markerSize: 7, label: "nodes");
            MainChart.Refresh();
        }

        private void AddOnChartInterpolationButton_Click(object sender, RoutedEventArgs e)
        {
            string function_type_string = InterpolationFunctionTypeComboBox.SelectedValue.ToString();
            InterpolationFunctionType interpolation_type = (InterpolationFunctionType)Enum.Parse(typeof(InterpolationFunctionType), function_type_string);
            IInterpolationFunction? interpolation_function = InterpolationBuilder.Build(_points, interpolation_type);
            if (interpolation_function is null) return;
            var xs = new List<double>();
            var ys = new List<double>();
            double start_x = new Expression(StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(EndXTextBox.Text.Trim()).calculate();
            for (double x = start_x; x <= end_x; x += 0.05)
            {
                var y = interpolation_function.Calculate(x);
                if (y is null) continue;

                xs.Add(x);
                ys.Add((double)y);
            }

            MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "interpolation");
            MainChart.Refresh();
        }

        private void AddOnChartDifferentiationButton_Click(object sender, RoutedEventArgs e)
        {
            string function_type_string = DifferentiationFunctionTypeComboBox.SelectedValue.ToString();
            DifferentiationFunctionType interpolation_type = (DifferentiationFunctionType)Enum.Parse(typeof(DifferentiationFunctionType), function_type_string);
            IDifferentiationFunction? interpolation_function = DifferentiationBuilder.Build(_points, interpolation_type, 0.05);
            if (interpolation_function is null) return;
            var xs = new List<double>();
            var ys = new List<double>();
            double start_x = new Expression(StartXTextBox.Text.Trim()).calculate();
            double end_x = new Expression(EndXTextBox.Text.Trim()).calculate();
            for (double x = start_x; x <= end_x; x += 0.05)
            {
                int derivative_degree = int.Parse(DerivativeDegreeTextBox.Text.Trim());
                var y = interpolation_function.Calculate(x, derivative_degree);
                if (y is null) continue;

                xs.Add(x);
                ys.Add((double)y);
            }

            MainChart.Plot.AddScatter(xs.ToArray(), ys.ToArray(), lineWidth: 4, markerSize: 0, label: "differentiation");
            MainChart.Refresh();
        }

        private void IntegrationCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void ClearChartButton_Click(object sender, RoutedEventArgs e)
        {
            MainChart.Plot.Clear();
            MainChart.Refresh();
        }
    }
}
