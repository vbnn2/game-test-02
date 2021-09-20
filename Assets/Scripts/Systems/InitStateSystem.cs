using DG.Tweening;
using ECS;

namespace Game
{
	public class InitStateSystem : ComponentSystem, IInitialize
	{
		private Constants _constants;

		public void Initialize()
		{
			DOTween.SetTweensCapacity(1000, 1);
			_world.Add(_world.UniqueEntity, new TurnTime { value = _constants.turnTime });
			_world.Add(_world.UniqueEntity, SimulationState.Initializing);
		}
	}
}