using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public class SimulationEndedDialog : MonoBehaviour
	{
		[SerializeField]
		private SkeletonDataAsset _attacker;

		[SerializeField]
		private SkeletonDataAsset _defender;

		[SerializeField]
		private SkeletonGraphic _anim;

		public void Show(bool isAttackerWin)
		{
			gameObject.SetActive(true);
			_anim.skeletonDataAsset = isAttackerWin ? _attacker : _defender;
			_anim.initialSkinName = "default";
			_anim.startingAnimation = Constants.kAnimIdleName;
			_anim.startingLoop = true;
			_anim.initialFlipX = true;
			_anim.Initialize(true);
		}

		public void OnReplay()
		{
			SceneManager.LoadScene("Simulation");
		}
	}
}