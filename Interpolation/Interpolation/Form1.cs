using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Interpolation
{
    public partial class Form1 : Form
    {
        double a = -1;
        double b = 1;
        int m = 5;
        int n = 20;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            //List<double> xIn = new List<double> { -2,-1,0,1,2 };
            //List<double> yIn = new List<double> { 3, 1, 1 };
            List<double> xIn = new List<double>();
            double h = (b - a) / (m - 1);
            for (int i = 0; i < m; i++)
            {
                double x = a + i * h;
                xIn.Add(x);
            }
            List<double> yIn = xIn.Select(x => ExactEval(x)).ToList();
            List<string> variables = new List<string> { "x" };
            List<double> constants = new List<double> { -1,0, 1,2,3,4,5};
            List<Function> functions = new List<Function>();

            Func<double, double, double> add = (x, y) => x + y;
            Func<double, double, double> subst = (x, y) => x - y;
            Func<double, double, double> multiplication = (x, y) => x * y;
            Func<double, double, double> division = (x, y) =>
            {
                if (y == 0) { return 1; }
                return x / y;
            };

            functions.Add(new Function(add, "+"));
            functions.Add(new Function(subst, "-"));
            functions.Add(new Function(multiplication, "*"));
            functions.Add(new Function(division, "%"));
            int minDepth = 1;
            int maxDepth = 2;
            double Pm = 0.01;
            double Pc = 1;
            double Psi = 0.2;
            SymbolRegression sr = new SymbolRegression(xIn, yIn, variables, constants, functions,
                minDepth, maxDepth, Pm, Pc, Psi);
            sr.Interpolate();

            var list = sr.population;
            richTextBox1.Text = "";
            richTextBox1.Text += "Min eroor = " + sr.absMinFunction+"\n\n";
            var list1 = sr.minFuncEachIteration;
            for (int i=0;i< list1.Count;i++)
            {
                richTextBox1.Text += String.Format("[{0}]:{1}\n",i,list1[i]);
            }
            richTextBox1.Text += "\n";
            foreach (var ind in list)
            {
                richTextBox1.Text += sr.PrintTree(ind, "", "") + "\n\n";
            }
            CreateChart(sr.minArgument);

        }

        private void CreateChart(Node minTree)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            CreateChartExact();
            string name = "Interpolation";
            chart1.Series.Add(name);
            chart1.Series[name].ChartType = SeriesChartType.Spline;
            chart1.Series[name].BorderWidth = 2;

            double h = (b - a) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double x = a + i * h;
                chart1.Series[name].Points.AddXY(x, minTree.Evaluate(x));
            }
        }
        private void CreateChartExact()
        {
            string name = "Exact";
            chart1.Series.Add(name);
            chart1.Series[name].ChartType = SeriesChartType.Spline;
            chart1.Series[name].BorderWidth = 2;

            double h = (b - a) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double x = a + i * h;
                chart1.Series[name].Points.AddXY(x, ExactEval(x));
            }
        }

        double ExactEval(double x)
        {
            //return (2*Math.Pow(x, 3) - Math.Pow(x,2)+ 4*x -2);
            //return (Math.Pow(x, 4) + 10);
            return (Math.Pow(x, 2) - x + 1);
        }
    }
}
