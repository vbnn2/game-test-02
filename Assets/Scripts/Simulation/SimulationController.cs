using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace Game
{
	public class SimulationController : MonoBehaviour
	{
		[SerializeField]
		private SimulationUI _ui;

		[SerializeField]
		private Constants _constants;

		[SerializeField]
		private GOPool _pool;
		
		private World _world;
		

		private void Awake()
		{
			_world = new World();
			_world.AddSystem(
				new InitBoardSystem(),
				new UpdateTurnSystem(),
				new UpdateHPUISystem(),
				new AttackSystem(),
				new MoveUnitSystem(),
				new CheckDeathSystem(),
				new GORecyclingSystem(),
				new DestroyEntitySystem()
			);

			_world.Inject(_ui);
			_world.Inject(_pool);
			_world.Inject(_constants);
			_world.Inject(new HexGid<int>());
			_world.Inject(new HexLayout(HexOrientation.kPointy, Vector2.one * _constants.hexSize, Vector2.zero));
		}

		private void Start()
		{
			_world.Initialize();
		}

		private void Update()
		{
			_world.Update();
		}

		private void LateUpdate()
		{
			_world.LateUpdate();
		}

		private void OnDestroy()
		{
			_world.CleanUp();
		}

		public void NextStep()
		{
			_world.CreateEntity(new NextTurn(), new DestroyEntity());
		}
	}
}
