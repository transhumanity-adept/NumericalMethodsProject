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
        Size = size;
    }
    public SquareMatrix(double [,] matrix)
    {
        _data = matrix;
        Size = matrix.GetLength(0);
    }

    public int Size { get; set; }

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

        for (int i = 0; i < _data.GetLength(0); i++)
        {
            double row_sum = 0;
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                row_sum += Math.Abs(_data[i,j]);
            }
            if (row_sum > max) max = row_sum;

        }
        return max;
    }

    SquareMatrix GetMatrixWithout(int index_row, int index_column)
    {
        SquareMatrix new_matrix = new SquareMatrix(_data.GetLength(0) - 1);
        
        for (int i = 0; i < new_matrix.Size; i++)
        {
            for (int j = 0; j < new_matrix.Size; j++)
            {
                if (i >= index_row && j >= index_column)
                    new_matrix[i, j] = _data[i + 1, j + 1];
                else if (i >= index_row)
                    new_matrix[i, j] = _data[i + 1, j];
                else if (j >= index_row)
                    new_matrix[i, j] = _data[i, j + 1];
                else
                    new_matrix[i, j] = _data[i, j];
            }
        }
        return new_matrix;
    }

    double GetDeterminant()
    {
        int size = _data.GetLength(0);

        switch (size)
        {
            case 1: return _data[0, 0];
            case 2: return _data[0, 0] * _data[1, 1] - _data[1, 0] * _data[0, 1];
            default:
                double determinant = 0;
                for (int column = 0; column < size; column++)
                {
                    determinant += (column % 2 == 0 ? 1 : -1) * _data[0, column] *  (GetMatrixWithout(0, column).GetDeterminant());
                }
                return determinant;
        }
    }

    SquareMatrix GetInverse()
    {
        double det = GetDeterminant();
        SquareMatrix algebraicadd = GetAlgebraicAddition();
        SquareMatrix trans = GetTranspose();

        SquareMatrix result = new SquareMatrix(_data.GetLength(0));


        for (int i = 0; i < result.Size; i++)
        {
            for (int j = 0; j < result.Size; j++)
            {
                result[i, j] = trans[i, j] / det;
            }
        }
        return result;
    }

    private SquareMatrix GetAlgebraicAddition()
    {
        for (int i = 0; i < _data.GetLength(0); i++)
        {
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                SquareMatrix without = GetMatrixWithout(i, j);
                this[i, j] = ((i + j) % 2 is 0 ? 1 : -1) * GetDeterminant();
            }
        }
        return this;
    }
    SquareMatrix GetTranspose()
    {
        SquareMatrix transpose = new SquareMatrix((double[,])_data.Clone());
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                _data[i,j] = transpose[i, j];
            }
        }
        return this;
    }
}
