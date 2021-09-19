using System.Linq;
using ECS;
using UnityEngine;

namespace Game
{
	public class InitBoardSystem : ComponentSystem, IInitialize
	{
		private SimulationUI _ui;
		private Constants _constants;
		private GOPool _pool;
		private HexGid<int> _hexGrid;
		private HexLayout _hexLayout;
		
		public void Initialize()
		{
			var totalRadius = _constants.attacker.startLine + _constants.defender.startLine + _constants.numSpace - 1;
			_hexGrid.InitHexagon(totalRadius, -1);

			// Init background
			foreach (var hex in _hexGrid.Keys)
			{
				CreateHexBackgound(hex);
			}

			// Init defender
			for (int i = 0; i < _constants.defender.startLine; i++)
			{
				var hexes = _hexGrid.GetRingPos(HexPos.kZero, i);
				foreach (var hex in hexes)
				{
					CreateDefender(hex);
				}
			}

			// Init attacker
			var attackerStartLine = _constants.defender.startLine + _constants.numSpace;
			var attackerEndLine = attackerStartLine + _constants.attacker.startLine;
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
			transform.position = _hexLayout.ToWorldPos(hex, 1f);
		}

		private void CreateDefender(HexPos hex)
		{
			// Create gameobject
			var transform = _pool.Get<Transform>("defender");
			transform.SetParent(_ui.root);
			transform.position = _hexLayout.ToWorldPos(hex);

			// Create entity
			var entity = _world.CreateEntity();
			_world.Add(entity, transform);
			_world.Add(entity, transform.GetComponent<HPRenderer>());
			_world.Add(entity, hex);
			_world.Add(entity, new Defender());
			_world.Add(entity, new BaseHP { value = _constants.defender.startHP });
			_world.Add(entity, new HP { value = _constants.defender.startHP });
			_world.Add(entity, new Number { value = Random.Range(0, 3) });

			_hexGrid[hex] = entity;

			Debug.Log($"-- Entity {entity} is defender");
		}

		private void CreateAttacker(HexPos hex)
		{
			var transform = _pool.Get<Transform>("attacker");
			transform.SetParent(_ui.root);
			transform.position = _hexLayout.ToWorldPos(hex);

			var entity = _world.CreateEntity();
			_world.Add(entity, transform);
			_world.Add(entity, transform.GetComponent<HPRenderer>());
			_world.Add(entity, hex);
			_world.Add(entity, new Attacker());
			_world.Add(entity, new BaseHP { value = _constants.attacker.startHP });
			_world.Add(entity, new HP { value = _constants.attacker.startHP });
			_world.Add(entity, new Number { value = Random.Range(0, 3) });

			_hexGrid[hex] = entity;

			Debug.Log($"@@ Entity {entity} is attacker");
		}
	}
}