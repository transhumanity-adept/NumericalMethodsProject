﻿using FParsec;

using System.Diagnostics.CodeAnalysis;

namespace NumericalMethods.Infrastructure.NonLinearEquationsSystems.Shared;
public abstract class VectorBase
{
	private readonly double[] _data;

	public VectorBase(double[] data)
	{
		_data = data;
	}
	public VectorBase(int size)
	{
		_data = new double[size];
	}

	public double this[int index]
	{
		get => _data[index];
		set => _data[index] = value;
	}

	public double GetNormM()
	{
		return _data.Select(vector => Math.Abs(vector)).Max();
	}

	public double[] ToArray()
	{
		return _data;
	}

	public int Size { get => _data.Length;  }
}