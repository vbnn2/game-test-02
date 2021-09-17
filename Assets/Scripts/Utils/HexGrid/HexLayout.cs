using UnityEngine;

namespace Game
{
	public struct HexOrientation
	{
		private const float kSqrt3 = 1.73205080757f;

		public float f0, f1, f2, f3;
		public float b0, b1, b2, b3;
		public float startAngle; // Multiply of 60°

		public HexOrientation(float f0, float f1, float f2, float f3,
							float b0, float b1, float b2, float b3,
							float startAngle)
		{
			this.f0 = f0;
			this.f1 = f1;
			this.f2 = f2;
			this.f3 = f3;
			this.b0 = b0;
			this.b1 = b1;
			this.b2 = b2;
			this.b3 = b3;
			this.startAngle = startAngle;
		}

		public static HexOrientation kPointy = new HexOrientation(
			kSqrt3, 		kSqrt3 / 2f, 	0f, 	3f / 2f,
			kSqrt3 / 3f, 	-1f / 3f, 		0f, 	2f / 3f,
			0.5f
		);

		public static HexOrientation kFlat = new HexOrientation(
			3f / 2f, 0f, kSqrt3 / 2f, kSqrt3,
			2f / 3f, 0f, -1f / 3f, kSqrt3 / 3f,
			0f
		);
	}

	public struct HexLayout
	{
		public HexOrientation orientation;
		public Vector2 size;
		public Vector2 origin;

		public HexLayout(HexOrientation orientation, Vector2 size, Vector2 origin)
		{
			this.orientation = orientation;
			this.size = size;
			this.origin = origin;
		}

		public Vector2 ToWorldPos(HexPos hex)
		{
			float x = (this.orientation.f0 * hex.q + this.orientation.f1 * hex.r) * this.size.x;
			float y = (this.orientation.f2 * hex.q + this.orientation.f3 * hex.r) * this.size.y;
			return new Vector2(x + origin.x, y + origin.y);
		}

		public HexPos ToHexPos(Vector2 pos)
		{
			Vector2 pt = (pos - origin) / size;
			float q = this.orientation.b0 * pt.x + this.orientation.b1 * pt.y;
			float r = this.orientation.b2 * pt.x + this.orientation.b3 * pt.y;
			return new HexPos((int)q, (int)r);
		}
	}
}