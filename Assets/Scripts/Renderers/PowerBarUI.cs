using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class PowerBarUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _textAtkPower;

		[SerializeField]
		private TMP_Text _textDefPower;

		[SerializeField]
		private TMP_Text _textTotalMinion;

		[SerializeField]
		private Image _imgAtk;

		[SerializeField]
		private Image _imgDef;

		public void SetPower(int atkPower, int defPower)
		{
			_textAtkPower.text = atkPower.ToString();
			_textDefPower.text = defPower.ToString();

			var ratio = (float)(atkPower) / (float)(atkPower + defPower);
			_imgAtk.rectTransform.DOAnchorMax(new Vector2(ratio, 1f), 0.25f);
			_imgDef.rectTransform.DOAnchorMin(new Vector2(ratio, 0f), 0.25f);
		}

		public void SetTotalMinion(int value)
		{
			_textTotalMinion.text = $"Total: {value}";
		}
	}
}