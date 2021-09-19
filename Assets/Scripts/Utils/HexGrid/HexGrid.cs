using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class HexGid<T> : Dictionary<HexPos, T>
	{
		public void InitHexagon(int radius, T defaultValue = default(T))
		{
			for (int q = -radius; q <= radius; q++)
			{
				int r1 = Mathf.Max(-radius, -q - radius);
				int r2 = Mathf.Min( radius, -q + radius);

				for (int r = r1; r <= r2; r++)
				{
					Add(new HexPos(q, r), defaultValue);
				}
			}
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
	}
}