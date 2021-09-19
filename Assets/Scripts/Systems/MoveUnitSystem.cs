using DG.Tweening;
using ECS;
using UnityEngine;

namespace Game
{
	public class MoveUnitSystem : ComponentSystem, IInitialize
	{
		private HexGid<int> _hexGrid;
		private HexLayout _hexLayout;

		public void Initialize()
		{
			_world.All<MoveUnit>().OnAdded.Bind(OnMoveUnit);
		}

		private void OnMoveUnit(int moveEntity)
		{
			_world.Get(moveEntity, out MoveUnit moveUnit);

			_hexGrid[moveUnit.toHex] = _hexGrid[moveUnit.fromHex];
			_hexGrid[moveUnit.fromHex] = -1;
			_world.Replace(_hexGrid[moveUnit.toHex], moveUnit.toHex);

			var entity = _hexGrid[moveUnit.toHex];
			var transform = _world.Get<Transform>(entity);
			transform.DOMove(_hexLayout.ToWorldPos(moveUnit.toHex), 0.5f);
		}
	}
}