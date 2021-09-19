using ECS;
using UnityEngine;

namespace Game
{
	public class CheckDeathSystem : ComponentSystem, IInitialize, IUpdate
	{
		private HexGid<int> _hexGrid;
		private ICollector _collector;

		public void Initialize()
		{
			_collector = _world.CreateCollector()
								.Trigger(_world.All<HP>(), TriggerEvent.Replaced)
								.Where(entity => _world.Get<HP>(entity).value <= 0);
		}

		public void Update()
		{
			foreach (var entity in _collector)
			{
				_world.Get(entity, out HexPos hex);
				_hexGrid[hex] = -1;
				_world.Add(entity, new DestroyEntity());

				Debug.Log($"Destroy entity: {entity}, ({hex.q}, {hex.r})");
			}

			_collector.Clear();
		}
	}
}