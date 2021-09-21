using System.Linq;
using ECS;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class InitBoardSystem : ComponentSystem, IInitialize
	{
		private SimulationUI _ui;
		private Constants _constants;
		private GOPool _pool;
		private HexGrid _hexGrid;
		private HexLayout _hexLayout;
		
		public void Initialize()
		{
			_world.All<InitBoardEvent>().OnAdded.Bind(CreateBoard);
		}

		private void CreateBoard(int evtEntity)
		{
			_world.Get(evtEntity, out InitBoardEvent evt);

			// Destroy old entities
			_world.DestroyEntities(_world.Any<HexBG, Attacker, Defender>());
			_hexGrid.Clear();

			// Calculate radius
			var totalRadius = evt.numAttacker + evt.numDefender + evt.numSpace - 1;
			_hexGrid.InitHexagon(totalRadius, Constants.kEmpty);

			// Init background
			for (int i = 0; i <= totalRadius; i++)
			{
				var hexes = _hexGrid.GetRingPos(HexPos.kZero, i);
				foreach (var hex in hexes)
				{
					CreateHexBackgound(hex);
				}
			}

			// Init defender
			for (int i = 0; i < evt.numDefender; i++)
			{
				var hexes = _hexGrid.GetRingPos(HexPos.kZero, i);
				foreach (var hex in hexes)
				{
					CreateDefender(hex);
				}
			}

			// Init attacker
			var attackerStartLine = evt.numDefender + evt.numSpace;
			var attackerEndLine = attackerStartLine + evt.numAttacker;
			for (int i = attackerStartLine; i < attackerEndLine; i++)
			{
				var hexes = _hexGrid.GetRingPos(HexPos.kZero, i);
				foreach (var hex in hexes)
				{
					CreateAttacker(hex);
				}
			}
		}

		private void CreateHexBackgound(HexPos hex)
		{
			var transform = _pool.Get<Transform>("hexagon");
			transform.SetParent(_ui.root);
			transform.localPosition = _hexLayout.ToWorldPos(hex, 1f);
			transform.localScale = Vector3.one;

			var entity = _world.CreateEntity();
			_world.Add(entity, transform);
			_world.Add(entity, new HexBG());
		}

		private void CreateDefender(HexPos hex)
		{
			// Create gameobject
			var transform = _pool.Get<Transform>("defender");
			transform.SetParent(_ui.root);
			transform.localPosition = _hexLayout.ToWorldPos(hex);
			transform.localScale = Vector3.one;

			// Create entity
			var entity = _world.CreateEntity();
			_world.Add(entity, transform);
			_world.Add(entity, transform.GetComponent<HPRenderer>());
			_world.Add(entity, transform.GetComponent<MeshRenderer>());
			_world.Add(entity, transform.GetComponent<SkeletonAnimation>());
			_world.Add(entity, new SpineUpdate());
			_world.Add(entity, hex);
			_world.Add(entity, new Defender());
			_world.Add(entity, new BaseHP { value = _constants.defender.startHP });
			_world.Add(entity, new HP { value = _constants.defender.startHP });
			_world.Add(entity, new Number { value = Random.Range(0, 3) });

			_hexGrid[hex] = entity;
		}

		private void CreateAttacker(HexPos hex)
		{
			var transform = _pool.Get<Transform>("attacker");
			transform.SetParent(_ui.root);
			transform.localPosition = _hexLayout.ToWorldPos(hex);
			transform.localScale = Vector3.one;

			var entity = _world.CreateEntity();
			_world.Add(entity, transform);
			_world.Add(entity, transform.GetComponent<HPRenderer>());
			_world.Add(entity, transform.GetComponent<MeshRenderer>());
			_world.Add(entity, transform.GetComponent<SkeletonAnimation>());
			_world.Add(entity, new SpineUpdate());
			_world.Add(entity, hex);
			_world.Add(entity, new Attacker());
			_world.Add(entity, new BaseHP { value = _constants.attacker.startHP });
			_world.Add(entity, new HP { value = _constants.attacker.startHP });
			_world.Add(entity, new Number { value = Random.Range(0, 3) });

			_hexGrid[hex] = entity;
		}
	}
}