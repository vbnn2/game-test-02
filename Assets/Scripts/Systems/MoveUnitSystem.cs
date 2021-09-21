using DG.Tweening;
using ECS;
using UnityEngine;

namespace Game
{
	public class MoveUnitSystem : ComponentSystem, IInitialize
	{
		private HexGrid _hexGrid;

		public void Initialize()
		{
			_world.All<MoveUnit>().OnAdded.Bind(OnMoveUnit);
		}

		private void OnMoveUnit(int moveEntity)
		{
			_world.Get(moveEntity, out MoveUnit moveUnit);

			_hexGrid[moveUnit.toHex] = _hexGrid[moveUnit.fromHex];
			_hexGrid[moveUnit.fromHex] = Constants.kEmpty;
			_world.Replace(_hexGrid[moveUnit.toHex], moveUnit.toHex);
		}
	}
}