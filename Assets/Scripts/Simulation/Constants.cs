using System;
using System.Collections.Generic;
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

		[Serializable]
		public class AttackDmg
		{
			public int value;
			public int dmg;
		}

		public Minion attacker;
		public Minion defender;
		public List<AttackDmg> attackDmgs;
		public int numSpace;
		public float hexSize;
		public float turnTime;
		public float frameSampleTime;
		public int frameSampleSkip;
		public float minZoomScale;
		public float maxZoomScale;

		public int GetDmg(int value)
		{
			return attackDmgs.Find(info => info.value == value)?.dmg ?? 0;
		}
	}
}