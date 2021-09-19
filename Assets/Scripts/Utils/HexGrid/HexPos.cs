using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public struct HexPos : IEquatable<HexPos>
	{
		public static HexPos kZero = new HexPos(0, 0);
		public static List<HexPos> kNeighbors = new List<HexPos>
		{
			new HexPos(1, 0), 	// E
			new HexPos(1, -1), 	// NE
			new HexPos(0, -1),	// NW
			new HexPos(-1, 0),	// W
			new HexPos(-1, 1),	// SW
			new HexPos(0, 1)	// SE
		};

		public int q;
		public int r;
		public int s;

		public HexPos(int q, int r)
		{
			this.q = q;
			this.r = r;
			this.s = -q-r;
		}
		
		public HexPos Neighbor(int direction)
		{
			return this + kNeighbors[direction % kNeighbors.Count];
		}

		public bool Equals(HexPos other)
		{
			return this == other;
		}

		public int Length()
		{
			return (int)((Mathf.Abs(this.q) + Mathf.Abs(this.r) + Mathf.Abs(this.s)) / 2);
		}

		public static int Distance(HexPos a, HexPos b)
		{
			return (a - b).Length();
		}

		public static HexPos operator+(HexPos a, HexPos b)
		{
			return new HexPos(a.q + b.q, a.r + b.r);
		}

		public static HexPos operator-(HexPos a, HexPos b)
		{
			return new HexPos(a.q - b.q, a.r - b.r);
		}

		public static HexPos operator*(HexPos a, int radius)
		{
			return new HexPos(a.q * radius, a.r * radius);
		}

		public static bool operator==(HexPos lhs, HexPos rhs)
		{
			return lhs.q == rhs.q && lhs.r == rhs.r;
		}

		public static bool operator!=(HexPos lhs, HexPos rhs)
		{
			return lhs.q != rhs.q || lhs.r != rhs.r;
		}

		public override bool Equals(object obj)
		{
			if (obj is HexPos other)
			{
				return this == other;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return this.q.GetHashCode() ^ (this.r.GetHashCode() << 2);
		}
	}
}