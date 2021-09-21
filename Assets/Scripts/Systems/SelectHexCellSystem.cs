using System.Text;
using ECS;
using Spine.Unity;
using UnityEngine;

namespace Game
{
	public class SelectHexCellSystem : ComponentSystem, IInitialize, IUpdate
	{
		private HexGrid _hexGrid;
		private HexLayout _layout;
		private SimulationUI _ui;
		private CameraSize _cameraSize;
		private int _selectingEntity;
		private StringBuilder _sb;
		private GOPool _pool;
		private SpineOutline _spineOutline;

		public void Initialize()
		{
			_sb = new StringBuilder();
			_world.All<MouseUp>().OnAdded.Bind(OnMouseUp);
		}

		private void OnMouseUp(int evtEntity)
		{
			_world.Get(evtEntity, out MouseUp mouseUp);

			// Check if it's a click
			if (Vector2.Distance(mouseUp.pos, mouseUp.startPos) > 0.05f)
				return;

			var worldPos = mouseUp.pos + _cameraSize.Camera.transform.position;
			var hex = _layout.ToHexPos(worldPos);

			if (_hexGrid.TryGet(hex, out int entity) && entity != Constants.kEmpty)
			{
				if (_spineOutline != null)
					_pool.Return(_spineOutline.transform);

				_selectingEntity = entity;
				
				_world.Get(entity, out Transform tranform);
				_spineOutline = _pool.Get<SpineOutline>("spine_outline");
				_spineOutline.transform.SetParent(tranform, false);
				_spineOutline.Active();
			}
			else
			{
				_selectingEntity = 0;
				_ui.minionDetailUI.Hide();
				_pool.Return(_spineOutline.transform);
				_spineOutline = null;
			}
		}

		public void Update()
		{
			if (_selectingEntity <= 0)
				return;
			
			// The isValid is not functioning, use hasAny instead
			// if (!_world.IsValid(_selectingEntity))
			
			if (!_world.Has<HP>(_selectingEntity))
			{
				_selectingEntity = 0;
				_pool.Return(_spineOutline.transform);
				_spineOutline = null;
				return;
			}

			_world.Get(_selectingEntity, out SkeletonAnimation anim);
			_world.Get(_selectingEntity, out HP hp);
			_world.Get(_selectingEntity, out BaseHP baseHP);
			_world.Get(_selectingEntity, out Number number);

			_sb.Length = 0;
			_sb.AppendLine($"Entity: {_selectingEntity}");
			_sb.AppendLine($"HP: {hp.value}/{baseHP.value}");
			_sb.AppendLine($"Number: {number.value}");

			_ui.minionDetailUI.Show(anim.skeletonDataAsset, _sb.ToString());
		}
	}
}