using ECS;
using UnityEngine;

namespace Game
{
	public class UpdateTurnTimeSystem : ComponentSystem, IUpdate
	{
		private Constants _constants;

		public void Update()
		{
			_world.Get(_world.UniqueEntity, out SimulationState state);
			if (state != SimulationState.Simulating)
				return;

			_world.Get(_world.UniqueEntity, out TurnTime turnTime);
			turnTime.value -= Time.deltaTime;

			if (turnTime.value <= 0)
			{
				turnTime.value = _constants.turnTime;
				_world.CreateEntity(new NextTurn(), new DestroyEntity());
			}

			_world.Replace(_world.UniqueEntity, turnTime);
		}
	}
}