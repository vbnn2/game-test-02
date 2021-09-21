using DG.Tweening;
using ECS;
using UnityEngine;

namespace Game
{
	public class CheckDeathSystem : ComponentSystem, IInitialize, IUpdate
	{
		private HexGrid _hexGrid;
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
				_hexGrid[hex] = Constants.kEmpty;
				_world.Add(entity, new DestroyEntity { delay = 0.75f });
			}

			_collector.Clear();
		}
	}
}