using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class HexGid<T> : Dictionary<HexPos, T>
	{
		public void InitHexagon(int radius)
		{
			for (int q = -radius; q <= radius; q++)
			{
				int r1 = Mathf.Max(-radius, -q - radius);
				int r2 = Mathf.Min( radius, -q + radius);

				for (int r = r1; r <= r2; r++)
				{
					Add(new HexPos(q, r), default(T));
				}
			}
		}
	}
}