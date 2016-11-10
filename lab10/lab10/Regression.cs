using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab10
{
    class Regression
    {
        public double k; // коефіцієнт k
        public double b; // коефіцієнт b
        public int amount = 8 + 10; // кількість точок
        public double start; // початкова точка
        public double h; // крок
        public double a0; // коефіцієнт альфа0
        public double a1; // крефіцієнт альфа1
        public Dictionary<double, double> function = new Dictionary<double, double>(); // дискретне представлення лінії1
        public Dictionary<double, double> function2 = new Dictionary<double, double>(); // дискретне представлення зашумлених даних
        public Dictionary<double, double> function3 = new Dictionary<double, double>(); // дискретне представлення регресійної функції


        // Конструктор
        public Regression(double k, double b, double start, double h)
        {
            this.k = k;
            this.b = b;
            this.start = start;
            this.h = h;

            // формування дискретного представлення лінії1
            for (int i = 0; i < amount; i++)
            {
                double x = this.start;
                double y = this.k * x + this.b;
                function.Add(x, y);
                this.start += this.h;
            }
        }

        // Обчислення
        public void Start(double mean)
        {
            Distort(mean);
            Evaluate();
            FormRegression();
        }

        // формування нормального розподілу
        private double[] RandomNormal(double mean)
        {
            double x;
            double[] array = new double[this.amount];

            Random rnd = new Random();

            double stddev = this.amount / 5.0;

            for(int i = 0; i < this.amount; i++)
            {
                double u1 = rnd.NextDouble();
                double u2 = rnd.NextDouble();

                double rndNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                    Math.Sin(2.0 * Math.PI * u2);
                x = (rndNormal * stddev) + mean;
                array[i] = x;
            }
            Array.Sort(array);
            return array;
        }

        // Зашумлення даних
        private void Distort(double mean)
        {
            double[] array = RandomNormal(mean);
            for (int i = 0; i < function.Count; i++)
                function2.Add(function.ElementAt(i).Key, function.ElementAt(i).Value + array[i]);
        }

        // Обчислення сум
        private double[] Sums()
        {
            double[] sum = new double[4] { 0.0, 0.0, 0.0, 0.0 };
            foreach (KeyValuePair<double, double> var in function2)
            {
                sum[0] += var.Key; // x
                sum[1] += var.Key * var.Key; // x^2
                sum[2] += var.Value; // y
                sum[3] += var.Key * var.Value; // x product y
            }
            return sum;
        }

        // Обчислення коефіцієнтів a0 та a1
        private void Evaluate()
        {
            double[] sums = Sums();
            double x = sums[0];
            double x2 = sums[1];
            double y = sums[2];
            double product = sums[3];
            a0 = (y * x2 - x * product) / (function2.Count * x2 - Math.Pow(x, 2));
            a1 = (function2.Count * 1.0 * product - x * y) / (function2.Count * x2 - Math.Pow(x,2));
        }

        // формування дискретного представлення регресійної функції
        private void FormRegression()
        {
            foreach(KeyValuePair<double, double> var in function2)
            {
                double y = a0 + a1 * var.Key;
                function3.Add(var.Key, y);
            }
        } 
    }
}
