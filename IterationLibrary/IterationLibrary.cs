using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class IterationSolutionNotFound : Exception
    {
        public IterationSolutionNotFound(string msg)
            : base("Решение не может быть найдено: \r\n" + msg)
        {
        }
    }

    public class LinearSystem
    {
        private static double[,] a_matrix;  // матрица A
        private static double[] x_vector;   // вектор неизвестных x
        private static double[] b_vector;   // вектор b
        private static double eps;          // порядок точности для сравнения вещественных чисел 
        private static int size;            // размерность задачи

        public LinearSystem(double[,] a_matrix, double[] b_vector)
            : this(a_matrix, b_vector, 0.0001)
        {
        }
        public LinearSystem(double[,] sa_matrix, double[] sb_vector, double ieps)
        {
            if (sa_matrix == null || sb_vector == null)
                throw new ArgumentNullException("Один из параметров равен null.");

            int b_length = sb_vector.Length;
            int a_length = sa_matrix.Length;
            if (a_length != b_length * b_length)
                throw new ArgumentException(@"Количество строк и столбцов в матрице A должно совпадать с количеством элементров в векторе B.");

            a_matrix = (double[,])sa_matrix.Clone();
            b_vector = (double[])sb_vector.Clone();
            x_vector = new double[b_length];
            size = b_length;
            eps = ieps;

            IterationSolve();
        }

        private static void IterationSolve()
        {

            double[] TempX = new double[size];
            double norm; // норма, определяемая как наибольшая разность компонент столбца иксов соседних итераций.
            Array.Clear(x_vector, 0, x_vector.Length);
            do
            {
                for (int i = 0; i < size; i++)
                {
                    TempX[i] = b_vector[i];
                    for (int g = 0; g < size; g++)
                    {
                        if (i != g)
                            TempX[i] = TempX[i] - a_matrix[i, g] * x_vector[g];
                    }
                    TempX[i] /= a_matrix[i, i];
                }
                norm = Math.Abs(x_vector[0] - TempX[0]);
                for (int h = 0; h < size; h++)
                {
                    if (Math.Abs(x_vector[h] - TempX[h]) > norm)
                        norm = Math.Abs(x_vector[h] - TempX[h]);
                    x_vector[h] = TempX[h];
                }
            } while (norm > eps);
        }

        public double[] XVector
        {
            get
            {
                return x_vector;
            }
        }
    }