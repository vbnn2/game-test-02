using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;

namespace Game
{
	public class UpdateTurnSystem : ComponentSystem, IInitialize
	{
		private HexGid<int> _hexGrid;

		public void Initialize()
		{
			_world.All<NextTurn>().OnAdded.Bind(OnNextTurn);
		}

		private void OnNextTurn(int _)
		{
			if (_world.All<Defender>().Count == 0 || _world.All<Attacker>().Count == 0)
			{
				UnityEngine.Debug.Log("Simulation is already ended! Something must wrong!");
				return;
			}

			// Defender attack
			_world.ForEachEntity((int defEntity, Defender _, HexPos hex) => {
				var neighbors = _hexGrid.GetRingPos(hex, 1);
				foreach (var neighbor in neighbors)
				{
					var neighborEntity = _hexGrid[neighbor];

					// Found attacker near by, attack it
					if (_world.Has<Attacker>(neighborEntity))
					{
						_world.CreateEntity(new Attack { fromEntity = defEntity, toEntity = neighborEntity },
											new DestroyEntity());
						return;
					}
				}
			});

			// Update attacker from closest center to farthest
			var atkEntities = _world.All<Attacker, HexPos>()
								.OrderBy(entity => _world.Get<HexPos>(entity).Length());
			foreach (var entity in atkEntities)
			{
				var hex = _world.Get<HexPos>(entity);
				var defHex = FindClosestDefender(hex);
				var distance = HexPos.Distance(hex, defHex);

				// Attack
				if (distance == 1)
				{
					_world.CreateEntity(
						new Attack { fromEntity = entity, toEntity = _hexGrid[defHex] },
						new DestroyEntity()
					);
				}
				else if (distance > 1)
				{
					var possibleMoves = _hexGrid.GetRingPos(hex, 1)
												.Where(neighbor => _hexGrid.TryGetValue(neighbor, out int value) && value == -1)
												.Where(neighbor => HexPos.Distance(neighbor, defHex) <= distance)
												.OrderBy(neighbor => HexPos.Distance(neighbor, defHex));

					if (possibleMoves.Count() > 0)
					{
						var bestMove = possibleMoves.First();
						_world.CreateEntity(
							new MoveUnit { fromHex = hex, toHex = bestMove },
							new DestroyEntity()
						);
					}
				}
			}
		}

		private HexPos FindClosestDefender(HexPos center)
		{
			for (int i = 1;; i++)
			{
				var hexes = _hexGrid.GetRingPos(center, i);
				foreach (var hex in hexes)
				{
					if (_hexGrid.TryGetValue(hex, out int entity) && _world.Has<Defender>(entity))
					{
						return hex;
					}
				}
			}

			return HexPos.kZero;
		}
	}
}