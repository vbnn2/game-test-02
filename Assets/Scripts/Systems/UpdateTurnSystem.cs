using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;

namespace Game
{
	public class UpdateTurnSystem : ComponentSystem, IInitialize
	{
		private HexGrid _hexGrid;

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

			var maxDefRadius = 0;

			// Defender attack
			_world.ForEachEntity((int defEntity, Defender _, HexPos hex) => {

				if (maxDefRadius < hex.lengthZero)
					maxDefRadius = hex.lengthZero;

				for (int i = 0; i < HexPos.kNeighbors.Count; i++)
				{
					var neighbor = hex.Neighbor(i);
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
								.OrderBy(entity => _world.Get<HexPos>(entity).lengthZero);
			var cachedGrid = CreateCachedGrid();
			foreach (var entity in atkEntities)
			{
				var hex = _world.Get<HexPos>(entity);
				var defHex = FindClosestDefender(hex, cachedGrid, maxDefRadius);
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
					var bestMove = hex;
					for (int n = 0; n < 6; n++)
					{
						var neighbor = hex.Neighbor(n);
						if (_hexGrid.TryGet(neighbor, out int value) && value == Constants.kEmpty && HexPos.Distance(neighbor, defHex) <= distance)
						{
							bestMove = neighbor;
							distance = HexPos.Distance(bestMove, defHex);
						}
					}

					if (bestMove != hex)
					{
						_world.CreateEntity(
							new MoveUnit { fromHex = hex, toHex = bestMove },
							new DestroyEntity()
						);
					}
				}
			}
		}

		private HexGrid CreateCachedGrid()
		{
			var cacheGrid = new HexGrid();
			cacheGrid.InitHexagon(_hexGrid.Radius);
			_world.ForEach((Defender _, HexPos hex) => {
				cacheGrid.Set(hex, 1);
			});

			return cacheGrid;
		}

		private HexPos FindClosestDefender(HexPos center, HexGrid cachedGrid, int maxDefRadius)
		{
			if (center.lengthZero - maxDefRadius > 3)
			{
				if (cachedGrid.Equals(HexPos.kZero, 1))
				{
					return HexPos.kZero;
				}
				else
				{
					var entities = _world.All<Defender, HexPos>();
					return _world.Get<HexPos>(entities[0]);
				} 
			}
			else
			{
				// Don't use HexGrid.GetRing for optimization
				for (int radius = 1;; radius++)
				{
					var hex = center + HexPos.kNeighbors[4] * radius;

					for (int n = 0; n < 6; n++)
					{
						for (int i = 0; i < radius; i++)
						{
							hex.Add(HexPos.kNeighbors[n]);
							if (cachedGrid.Equals(hex, 1))
							{
								return hex;
							}
						}
					}
				}
			}
		}
	}
}