using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPRV_lab6_WPF.task3
{
    class JacobiSolver
    {
        public static double[] Solve(double[,] A, double[] b, double tolerance)
        {
            int n = A.GetLength(0);
            double[] x = new double[n];
            double[] x_new = new double[n];
            double[] residual = new double[n];
            double maxResidual = double.MaxValue;
            int maxIterations = 1000;
            int iteration = 0;

            // Инициализация начального приближения
            for (int i = 0; i < n; i++)
                x[i] = 0;

            // Итерационный процесс метода Якоби
            while (maxResidual > tolerance && iteration < maxIterations)
            {
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                            sum += A[i, j] * x[j];
                    }
                    x_new[i] = (b[i] - sum) / A[i, i];
                }

                // Вычисление нормы разницы между текущим и предыдущим приближениями
                maxResidual = 0;
                for (int i = 0; i < n; i++)
                {
                    residual[i] = Math.Abs(x_new[i] - x[i]);
                    if (residual[i] > maxResidual)
                        maxResidual = residual[i];
                }

                // Обновление приближения
                for (int i = 0; i < n; i++)
                    x[i] = x_new[i];

                iteration++;
            }

            return residual;
        }

        static void CalculateIterations(double[,] A, double[] b, double[] x, double[] xNew, double[] residual, int start, int end)
        {
            int n = A.GetLength(0);
            for (int i = start; i < end; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        sum += A[i, j] * x[j];
                }
                xNew[i] = (b[i] - sum) / A[i, i];
                residual[i] = Math.Abs(xNew[i] - x[i]);
            }
        }

    }
}
