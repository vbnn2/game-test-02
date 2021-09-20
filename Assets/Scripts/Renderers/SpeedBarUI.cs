using TMPro;
using UnityEngine;

namespace Game
{
	public class SpeedBarUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _textSpeed;

		private void Awake()
		{
			OnSpeedChanged(Time.timeScale);
		}

		public void OnSpeedChanged(float value)
		{
			_textSpeed.text = $"{value:0.0}x";
			Time.timeScale = value;
		}
	}
}