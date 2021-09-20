using UnityEngine;

namespace Game
{
	public struct MouseDown
	{
		public Vector3 pos;
	}

	public struct MouseMoved
	{
		public Vector3 startPos;
		public Vector3 lastPos;
		public Vector3 pos;
	}

	public struct MouseUp
	{
		public Vector3 startPos;
		public Vector3 lastPos;
		public Vector3 pos;
	}

	public struct MouseZoomed
	{
		public Vector3 pos;
		public float delta;
	}
}