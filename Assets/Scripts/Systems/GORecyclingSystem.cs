using ECS;
using UnityEngine;

namespace Game
{
	public class GORecyclingSystem : ComponentSystem, IInitialize
	{
		private GOPool _pool;

		public void Initialize()
		{
			_world.All<Transform>().OnPreRemoved.Bind(entity =>
			{
				var transform = _world.Get<Transform>(entity);
				_pool.Return(transform);
			});
		}
	}
}