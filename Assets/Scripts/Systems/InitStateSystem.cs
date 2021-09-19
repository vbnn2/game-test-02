using ECS;

namespace Game
{
	public class InitStateSystem : ComponentSystem, IInitialize
	{
		private Constants _constants;

		public void Initialize()
		{
			_world.Add(_world.UniqueEntity, new TurnTime { value = _constants.turnTime });
			_world.Add(_world.UniqueEntity, SimulationState.Initializing);
		}
	}
}