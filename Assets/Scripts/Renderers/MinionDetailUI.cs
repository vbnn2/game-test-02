using Spine.Unity;
using TMPro;
using UnityEngine;

namespace Game
{
	public class MinionDetailUI : MonoBehaviour
	{
		[SerializeField]
		private SkeletonGraphic _anim;
		
		[SerializeField]
		private TMP_Text _textInfo;

		public void Show(SkeletonDataAsset asset, string info)
		{
			gameObject.SetActive(true);

			if (_anim.skeletonDataAsset != asset)
			{
				_anim.skeletonDataAsset = asset;
				_anim.initialSkinName = "default";
				_anim.startingAnimation = Constants.kAnimIdleName;
				_anim.startingLoop = true;
				_anim.initialFlipX = true;
				_anim.Initialize(true);
			}
			

			_textInfo.text = info;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}