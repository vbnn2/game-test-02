using DG.Tweening;
using ECS;
using UnityEngine;

namespace Game
{
	public class DeathAnimSystem : ComponentSystem, IInitialize, IUpdate
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
				_world.Get(entity, out Transform transform);
				transform.DOScale(0, 0.5f).SetDelay(0.25f);
			}

			_collector.Clear();
		}
	}
}