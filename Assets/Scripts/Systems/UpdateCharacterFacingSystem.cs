using ECS;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class UpdateCharacterFacingSystem : ComponentSystem, IInitialize, IUpdate
	{
		public void Initialize()
		{
			_world.All<Attacker, Transform, SkeletonAnimation>().OnAdded.Bind(entity => {
				_world.Get(entity, out Transform transform, out SkeletonAnimation anim);
				anim.skeleton.FlipX = transform.position.x < 0;
			});

			_world.All<Defender, Transform, SkeletonAnimation>().OnAdded.Bind(entity => {
				_world.Get(entity, out Transform transform, out SkeletonAnimation anim);
				anim.skeleton.FlipX = transform.position.x > 0;
			});
		}

		public void Update()
		{
			_world.ForEach((Attacker _, Transform transform, SkeletonAnimation anim) => {
				anim.skeleton.FlipX = transform.position.x < 0;
			});

			_world.ForEach((Defender _, Transform transform, SkeletonAnimation anim) => {
				anim.skeleton.FlipX = transform.position.x > 0;
			});
		}
	}
}