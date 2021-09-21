using ECS;
using UnityEngine;

namespace Game
{
	public class SimulationController : MonoBehaviour
	{
		[SerializeField]
		private CameraSize _cameraSize;

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
				new InitStateSystem(),
				new InitBoardSystem(),
				new UpdateTurnTimeSystem(),
				new UpdateTurnSystem(),
				new UpdateHPUISystem(),
				new AttackSystem(),
				new AttackAnimSystem(),
				new MoveUnitSystem(),
				new MoveUnitAnimSystem(),
				new CheckDeathSystem(),
				new DeathAnimSystem(),
				new UpdatePowerBarUISystem(),
				new CheckSimulationEndedSystem(),
				new SelectHexCellSystem(),

				new MouseInputSystem(),
				new PanAndZoomLayerSystem(),
				new UpdateSpineSystem(),
				new ExpandBoardSystem(),
				new UpdateCharacterFacingSystem(),

				new GORecyclingSystem(),
				new DestroyEntitySystem()
			);

			_world.Inject(_cameraSize);
			_world.Inject(_ui);
			_world.Inject(_pool);
			_world.Inject(_constants);
			_world.Inject(new HexGrid());
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

		public void OnPause()
		{
			_world.Get(_world.UniqueEntity, out SimulationState state);
			if (state == SimulationState.Simulating)
			{
				_ui.textPause.text = "Resume";
				_world.Replace(_world.UniqueEntity, SimulationState.Pausing);
			}
			else if (state == SimulationState.Pausing)
			{
				_ui.textPause.text = "Pause";
				_world.Replace(_world.UniqueEntity, SimulationState.Simulating);
			}
		}
	}
}
