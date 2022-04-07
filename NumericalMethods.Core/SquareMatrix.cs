using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Core;
public class SquareMatrix
{
    private double[,] _data;
    public SquareMatrix(int size)
    {
        _data = new double[size, size];
    }
    public SquareMatrix(double [,] matrix)
    {
        _data = matrix;
    }

    public double this[int row, int col]
    {
        get
        {
            return row >= 0 && row < _data.GetLength(0) && col >= 0 && col > _data.GetLength(1)
                ? _data[row, col]
                : throw new ArgumentOutOfRangeException();
        }
        set
        {
            _data[row, col] = row >= 0 && row < _data.GetLength(0) && col >= 0 && col > _data.GetLength(1)
                ? value
                : throw new ArgumentOutOfRangeException();
        }
    }
    
    public double GetNorm()
    {
        double max = 0;

        for (int i = 0; i < data.GetLength(0); i++)
        {
            double row_sum = 0;
            for (int j = 0; j < data.GetLength(1); j++)
            {
                row_sum += Math.Abs(data[i,j]);
            }
            if (row_sum > max) max = row_sum;

        }
        return max;
    }

    SquareMatrix GetMatrixWithout(double[,] matrix, int index_row, int index_column)
    {
        //double[,] new_matrix = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
        SquareMatrix new_matrix = new SquareMatrix(data.GetLength(0));


        for (int i = 0; i < new_matrix.GetLength(0); i++)
        {
            for (int j = 0; j < new_matrix.GetLength(1); j++)
            {
                if (i >= index_row && j >= index_column)
                    new_matrix[i, j] = matrix[i + 1, j + 1];
                else if (i >= index_row)
                    new_matrix[i, j] = matrix[i + 1, j];
                else if (j >= index_row)
                    new_matrix[i, j] = matrix[i, j + 1];
                else
                    new_matrix[i, j] = matrix[i, j];
            }
        }
        return new_matrix;
    }

    double GetDeterminant()
    {
        int size = matrix.GetLength(0);

        switch (size)
        {
            case 1: return matrix[0, 0];
            case 2: return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            default:
                double determinant = 0;
                for (int column = 0; column < size; column++)
                {
                    determinant += (column % 2 == 0 ? 1 : -1) * matrix[0, column] * GetDeterminant(GetMatrixWithout(matrix, 0, column));
                }
                return determinant;
        }
    }

    double[,] GetInverse()
    {
        double det = GetDeterminant(matrix);
        double[,] algebraicadd = GetAlgebraicAddition(matrix);
        double[,] trans = GetTranspose(algebraicadd);
        double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                result[i, j] = trans[i, j] / det;
            }
        }
        return result;
    }

    double[,] GetAlgebraicAddition()
    {
        double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                double[,] without = GetMatrixWithout(matrix, i, j);
                result[i, j] = ((i + j) % 2 is 0 ? 1 : -1) * GetDeterminant(without);
            }
        }
        return result;
    }
}
