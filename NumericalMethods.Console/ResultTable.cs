using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Console
{
    public class ResultTable
    {
        private int _differentialOrder;
        // TODO: Возвращать неизменяемую коллекцию
        public List<string> Header { get; }
        private List<List<double>> Body { get; }
        public ResultTable(int differentialOrder)
        {
            _differentialOrder = differentialOrder;
            Header = new List<string>(differentialOrder + 1);
            Header.Add("x");
            for (int i = 0; i < differentialOrder + 1; i++)
            {
                Header.Add($"y{i}");
            }
            Body = new List<List<double>>();
        }

        public List<(double x, double yi)> this[string variableName]
        {
            get
            {
                int index = Header.IndexOf(variableName);
                if (index == -1) throw new ArgumentException($"Invalid {nameof(variableName)}");
                List<(double x, double yi)> result = new List<(double x, double yi)>();
                foreach (var row in Body)
                {
                    result.Add((row[0], row[index]));
                }
                return result;
            }
        }

        public ResultTable Add(double x, Dictionary<string, double> ys)
        {
            Dictionary<string, double> sortedYs = ys.OrderBy(y => y.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            List<double> addedList = new List<double>() { x };
            addedList.AddRange(sortedYs.Select(pair => pair.Value));
            Body.Add(addedList);
            return this;
        }
    }
}
