using ECS;

namespace Game
{
	public class CheckSimulationEndedSystem : ComponentSystem, IInitialize
	{
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
				_world.Replace(_world.UniqueEntity, SimulationState.Ended);
				UnityEngine.Debug.Log("Simulation Ended");
			}
		}
	}
}