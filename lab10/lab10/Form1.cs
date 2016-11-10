using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Regression rg = new Regression(1, 0, 1, 1);

            rg.Start(0);

            // Заповнення даних для points(line1) та line1
            foreach(KeyValuePair<double, double> var in rg.function)
            {
                this.chart1.Series["Points(line1)"].Points.AddXY(var.Key, var.Value);
                this.chart1.Series["line1"].Points.AddXY(var.Key, var.Value);
               
            }

            // Заповнення даних для points(distort)
            foreach(KeyValuePair<double, double> var in rg.function2)
                this.chart1.Series["Points2(distort)"].Points.AddXY(var.Key, var.Value);

            // Заповнення даних для points(regression) та regression
            foreach (KeyValuePair<double, double> var in rg.function3)
            {
                this.chart1.Series["Points3(regression)"].Points.AddXY(var.Key, var.Value);
                this.chart1.Series["regression"].Points.AddXY(var.Key, var.Value);
            }

            // Виведення коефіцієнтів a1, a0
            this.line1.Text = Convert.ToString(rg.k + "; " + rg.b);
            this.regression.Text = Convert.ToString(Math.Round(rg.a1, 3) + "; " + Math.Round(rg.a0, 3));
        }
    }
}
