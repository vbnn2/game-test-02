using ECS;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class AttackAnimSystem : ComponentSystem, IInitialize
	{
		private Constants _constants;

		public void Initialize()
		{
			_world.All<Attack>().OnAdded.Bind(OnAttack);
		}

		private void OnAttack(int atkEntity)
		{
			_world.Get(atkEntity, out Attack attack);
			_world.Get(attack.fromEntity, out SkeletonAnimation anim);
			var trackEntry = anim.AnimationState.SetAnimation(0, Constants.kAnimAttackName, false);
			trackEntry.TimeScale = 3;

			anim.AnimationState.AddAnimation(0, Constants.kAnimIdleName, true, 0);
		}
	}
}