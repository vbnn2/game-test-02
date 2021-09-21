using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class HexGrid
	{
		private const int kNull = -1;
		public int[][] _data;
		private int _radius;
		private int _size;

		public int Radius => _radius;

		public void InitHexagon(int radius, int defaultValue = 0)
		{
			_radius = radius;

			_size = _radius * 2 + 1;
			_data = new int[_size][];
			for (int i = 0; i < _size; i++)
			{
				_data[i] = new int[_size];
				for (int j = 0; j < _size; j++)
				{
					_data[i][j] = kNull;
				}
			}

			for (int q = -radius; q <= radius; q++)
			{
				int r1 = Mathf.Max(-radius, -q - radius);
				int r2 = Mathf.Min( radius, -q + radius);

				for (int r = r1; r <= r2; r++)
				{
					// Add(new HexPos(q, r), defaultValue);
					Set(new HexPos(q, r), defaultValue);
				}
			}
		}

		public void Clear()
		{
			_radius = -1;
			_data = null;
		}

		public int this[HexPos hex]
		{
			get => Get(hex);
			set => Set(hex, value);
		}

		public void Set(HexPos hex, int value)
		{
			_data[hex.q + _radius][hex.r + _radius] = value;
		}

		public int Get(HexPos hex)
		{
			return _data[hex.q + _radius][hex.r + _radius];
		}

		public List<HexPos> GetRingPos(HexPos center, int radius)
		{
			if (radius == 0)
			{
				return new List<HexPos> { center };
			}

			var results = new List<HexPos>();
			var hex = center + HexPos.kNeighbors[4] * radius;

			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					results.Add(hex);
					hex = hex.Neighbor(i);
				}
			}

			return results;
		}

		public bool TryGet(HexPos hex, out int value)
		{
			if (hex.lengthZero <= _radius)
			{
				value = Get(hex);
				return true;
			}

			value = -1;
			return false;
		}

		public bool Equals(HexPos hex, int value)
		{
			if (hex.lengthZero <= _radius)
			{
				return Get(hex) == value;
			}

			return false;
		}

		// public IEnumerator<int> GetEnumerator()
		// {
		// 	for (int q = 0; q < _size; q++)
		// 	{
		// 		for (int r = 0; r < _size; r++)
		// 		{
		// 			var value = 
		// 		}
		// 	}
		// }

		// IEnumerator IEnumerable.GetEnumerator()
		// {
		// 	return GetEnumerator();
		// }
	}
}