using System;
using UnityEngine;

namespace Game
{
	[CreateAssetMenu]
	public class Constants : ScriptableObject
	{
		[Serializable]
		public class Minion
		{
			public int startHP;
			public int startLine;
		}

		public Minion attacker;
		public Minion defender;
		public int numSpace;
		public float hexSize;
	}
}