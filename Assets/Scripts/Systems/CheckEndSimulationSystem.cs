using ECS;
using UnityEngine;

namespace Game
{
	public class CheckSimulationEndedSystem : ComponentSystem, IInitialize
	{
		private SimulationUI _ui;

		public void Initialize()
		{
			_world.All<Attacker>().OnPostRemoved.Bind(CheckEnded);
			_world.All<Defender>().OnPostRemoved.Bind(CheckEnded);
		}

		private void CheckEnded(int _)
		{
			_world.Get(_world.UniqueEntity, out SimulationState state);
			if (state != SimulationState.Simulating)
				return;

			if (_world.All<Defender>().Count == 0 || _world.All<Attacker>().Count == 0)
			{
				Time.timeScale = 1;
				_world.Replace(_world.UniqueEntity, SimulationState.Ended);
				_ui.simulationEndedDialog.Show(_world.All<Defender>().Count == 0);
				UnityEngine.Debug.Log("Simulation Ended");
			}
		}
	}
}