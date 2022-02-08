using NumericalMethods.Approximation.Interpolations;
using NumericalMethods.Approximation.Interpolations.Interfaces;

using org.mariuszgromada.math.mxparser;

using ScottPlot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumericalMethods.WPFApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<InterpolationNode> nodes;
        private const double StartX = -4 * Math.PI;
        private const double EndX = 4 * Math.PI;
        private const double Step = Math.PI / 2;
        public MainWindow()
        {
            InitializeComponent();
            Function function = new Function("f(x) = sin(x)");
            nodes = new List<InterpolationNode>();

            for (double x = StartX; x <= EndX; x += Step)
            {
                nodes.Add(new InterpolationNode(x, function.calculate(x)));
            }

            foreach (var item in Enum.GetValues(typeof(InterpolationFunctionType)))
            {
                Selector.Items.Add(item.ToString());
            }

            Width = 1300;
            Height = 750;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MainChart.Plot.Clear();
            MainChart.Refresh();
            IInterpolationFunction? interpolation_function = InterpolationBuilder.Build(nodes, (InterpolationFunctionType)Enum.Parse(typeof(InterpolationFunctionType), Selector.SelectedValue.ToString()));
            if (interpolation_function is null) return;
            var interpolation_xs = new List<double>();
            var interpolation_ys = new List<double>();
            for (double x = StartX; x <= EndX; x += 0.1)
            {
                var y = interpolation_function.Calculate(x);
                if (y is null) continue;

                interpolation_xs.Add(x);
                interpolation_ys.Add((double)y);
            }

            MainChart.Plot.AddScatter(interpolation_xs.ToArray(), interpolation_ys.ToArray(), lineWidth: 3, markerSize: 4, label: "interpolation");
            var xs = nodes.Select(node => node.X).ToArray();
            var ys = nodes.Select(node => node.Y).ToArray();
            MainChart.Plot.AddScatter(xs, ys, lineWidth: 0, markerSize: 5, label: "data");
            MainChart.Plot.Legend();
            MainChart.Refresh();
        }
    }
}
