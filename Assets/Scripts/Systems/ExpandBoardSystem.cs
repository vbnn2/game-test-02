using ECS;
using UnityEngine;

namespace Game
{
	public class ExpandBoardSystem : ComponentSystem, IInitialize, IUpdate
	{
		private Constants _constants;

		private int _numAttacker;
		private int _numDefender;
		private int _numSpace;
		private int _lastFrameCount;
		private float _time;
		private float _delay;

		public void Initialize()
		{
			_numAttacker = _constants.attacker.startLine;
			_numDefender = _constants.defender.startLine;
			_numSpace = _constants.numSpace;
			_time = _constants.frameSampleTime;
			_delay = _constants.frameSampleDelay;
			_lastFrameCount = Time.frameCount;

			CreateBoardEvent();
		}

		public void Update()
		{
			_world.Get(_world.UniqueEntity, out SimulationState state);
			if (state != SimulationState.Initializing)
				return;
			
			if (_delay > 0)
			{
				_delay -= Time.deltaTime;
				return;
			}
			
			_time -= Time.deltaTime;
			if (_time <= 0)
			{
				var totalFrame = Time.frameCount - _lastFrameCount;
				var fps = totalFrame / _constants.frameSampleTime;

				Debug.Log($"Sample FPS: {fps}");
				if (fps >= 30f)
				{
					_numAttacker += 1;
					_numDefender += 1;
					CreateBoardEvent();

					_time = _constants.frameSampleTime;
					_delay = _constants.frameSampleDelay;
					_lastFrameCount = Time.frameCount;
				}
				else
				{
					_world.Replace(_world.UniqueEntity, SimulationState.Simulating);
					UnityEngine.Debug.Log("Simulation Start");
				}
			}
		}

		private void CreateBoardEvent()
		{
			_world.CreateEntity(
				new InitBoardEvent
				{
					numAttacker = _numAttacker,
					numDefender = _numDefender,
					numSpace = _numSpace
				},
				new DestroyEntity()
			);
		}
	}
}