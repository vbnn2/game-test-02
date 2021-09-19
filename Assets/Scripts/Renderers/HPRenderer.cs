using DG.Tweening;
using UnityEngine;

namespace Game
{
	public class HPRenderer : MonoBehaviour
	{
		[SerializeField]
		private Transform _hpBar;

		private int _baseHP;

		public void SetBaseHP(int value)
		{
			_baseHP = value;
			_hpBar.localScale = Vector3.one;
		}

		public void SetHP(int value)
		{
			float ratio = (float)value / (float)_baseHP;
			ratio = Mathf.Max(ratio, 0);

			_hpBar.DOScaleX(ratio, 0.25f);
		}
	}
}