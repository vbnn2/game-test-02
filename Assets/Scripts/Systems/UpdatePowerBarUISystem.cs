using ECS;

namespace Game
{
	public class UpdatePowerBarUISystem : ComponentSystem, IInitialize, IUpdate
	{
		private SimulationUI _ui;
		private ICollector _collector;

		public void Initialize()
		{
			_collector = _world.CreateCollector()
								.Trigger(_world.All<HP>(), TriggerEvent.Added | TriggerEvent.Replaced | TriggerEvent.Removed);
		}

		public void Update()
		{
			if (_collector.Count == 0)
				return;
			
			var totalAtkHp = 0;
			var totalDefHp = 0;

			_world.ForEach((Attacker _, HP hp) => {
				totalAtkHp += hp.value;
			});

			_world.ForEach((Defender _, HP hp) => {
				totalDefHp += hp.value;
			});

			_ui.powerBarUI.SetPower(totalAtkHp, totalDefHp);
			_ui.powerBarUI.SetTotalMinion(_world.Any<Attacker, Defender>().Count);

			_collector.Clear();
		}
	}
}