using DG.Tweening;
using ECS;
using UnityEngine;

namespace Game
{
	public class MoveUnitAnimSystem : ComponentSystem, IInitialize
	{
		private HexGrid _hexGrid;
		private HexLayout _hexLayout;

		public void Initialize()
		{
			_world.All<MoveUnit>().OnAdded.Bind(OnMoveUnit);
		}

		private void OnMoveUnit(int moveEntity)
		{
			_world.Get(moveEntity, out MoveUnit moveUnit);

			var entity = _hexGrid[moveUnit.toHex];
			var transform = _world.Get<Transform>(entity);
			transform.DOLocalMove(_hexLayout.ToWorldPos(moveUnit.toHex), 0.5f);
		}
	}
}