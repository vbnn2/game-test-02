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
		public int lengthZero;

		public HexPos(int q, int r)
		{
			this.q = q;
			this.r = r;
			this.s = -q-r;
			this.lengthZero = (Abs(this.q) + Abs(this.r) + Abs(this.s)) / 2;
		}
		
		public HexPos Neighbor(int direction)
		{
			return this + kNeighbors[direction % kNeighbors.Count];
		}

		public bool Equals(HexPos other)
		{
			return this.q == other.q && this.r == other.r;
		}

		public int Length()
		{
			return (Abs(this.q) + Abs(this.r) + Abs(this.s)) / 2;
		}

		public static int Distance(HexPos a, HexPos b)
		{
			return (Abs(a.q - b.q) + Abs(a.r - b.r) + Abs(a.s - b.s)) / 2;
		}

		private static int Abs(int value)
		{
			return value > 0 ? value : -value;
		}

		public static HexPos operator+(HexPos a, HexPos b)
		{
			return new HexPos(a.q + b.q, a.r + b.r);
		}

		public void Add(HexPos other)
		{
			this.q += other.q;
			this.r += other.r;
			this.s += other.s;
			this.lengthZero = (Abs(this.q) + Abs(this.r) + Abs(this.s)) / 2;
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
			return this.q << 16 + this.r;
		}
	}
}