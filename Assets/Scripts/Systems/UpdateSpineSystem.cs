using ECS;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class UpdateSpineSystem : ComponentSystem, IInitialize, IUpdate, ILateUpdate
	{
		private CameraSize _cameraSize;
		private Constants _constants;

		public void Initialize()
		{
			_world.All<SkeletonAnimation>().OnAdded.Bind(entity => {
				_world.Get(entity, out SkeletonAnimation anim);
				anim.Initialize(false);
				anim.clearStateOnDisable = false;
				anim.enabled = false;
			});
		}

		public void Update()
		{
			var deltaTime = Time.deltaTime;
			_world.ForEach((ref SpineUpdate update, SkeletonAnimation anim, MeshRenderer renderer) =>
			{
				update.value = renderer.isVisible;
				if (update.value)
					anim.Update(deltaTime);
			});
		}

		public void LateUpdate()
		{
			_world.ForEach((SkeletonAnimation anim, SpineUpdate update) => {
				if (update.value)
					anim.LateUpdate();
			});
		}
	}
}