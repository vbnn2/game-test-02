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
			_hexGrid.InitHexagon(totalRadius);

			// Init background
			foreach (var hex in _hexGrid.Keys)
			{
				var transform = _pool.Get<Transform>("hexagon");
				transform.SetParent(_ui.root);
				transform.position = _hexLayout.ToWorldPos(hex);
			}

			// Init defender
			var defenderHexes = _hexGrid.Keys.Where(hex => hex.Length() < _constants.defender.startLine);
			foreach (var hex in defenderHexes)
			{
				var transform = _pool.Get<Transform>("defender");
				transform.SetParent(_ui.root);
				transform.position = _hexLayout.ToWorldPos(hex);
			}

			// Init attacker
			var attackerStartLine = _constants.defender.startLine + _constants.numSpace;
			var attackerEndLine = attackerStartLine + _constants.attacker.startLine;
			var attackerHexes = _hexGrid.Keys.Where(hex => {
				var length = hex.Length();
				return length >= attackerStartLine && length < attackerEndLine;
			});
			foreach (var hex in attackerHexes)
			{
				var transform = _pool.Get<Transform>("attacker");
				transform.SetParent(_ui.root);
				transform.position = _hexLayout.ToWorldPos(hex);
			}
		}
	}
}