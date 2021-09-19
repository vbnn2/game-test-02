using ECS;
using UnityEngine;

namespace Game
{
	public class DestroyEntitySystem : ComponentSystem, ILateUpdate
	{
		public void LateUpdate()
		{
			var deltaTime = Time.deltaTime;
			_world.ForEachEntity((int entity, DestroyEntity destroyEntity) =>
			{
				destroyEntity.delay -= deltaTime;
				if (destroyEntity.delay <= 0)
				{
					_world.DestroyEntity(entity);
				}
			});
		}
	}
}