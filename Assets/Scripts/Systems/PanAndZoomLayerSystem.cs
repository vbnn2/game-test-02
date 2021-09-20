using ECS;
using UnityEngine;

namespace Game
{
	public class PanAndZoomLayerSystem : ComponentSystem, IInitialize, IUpdate
	{
		private const float kDecelerationRate = 0.135f;
		
		private SimulationUI _ui;
		private Constants _constants;
		private CameraSize _cameraSize;

		private bool _needUpdateInertia;
		private float _moveSpeed;
		private Vector2 _moveVel;
		private float _minScale;

		public void Initialize()
		{
			_world.All<MouseDown>().OnAdded.Bind(OnMouseDown);
			_world.All<MouseMoved>().OnAdded.Bind(OnMouseMoved);
			_world.All<MouseUp>().OnAdded.Bind(OnMouseUp);
			_world.All<MouseZoomed>().OnAdded.Bind(OnMouseZoomed);
			_world.All<InitBoardEvent>().OnAdded.Bind(OnBoardSizeChanged);
		}

		private void OnBoardSizeChanged(int entity)
		{
			_world.Get(entity, out InitBoardEvent evt);

			var radius = evt.numAttacker + evt.numDefender + evt.numSpace;
			_ui.rootTL.localPosition = new Vector2(-radius * Mathf.Sqrt(3),  radius * 1.5f) * _constants.hexSize * 1.2f;
			_ui.rootBR.localPosition = new Vector2( radius * Mathf.Sqrt(3), -radius * 1.5f) * _constants.hexSize * 1.2f;
			var diff = _ui.rootBR.localPosition - _ui.rootTL.localPosition;
			_minScale = Mathf.Max(_cameraSize.Width / Mathf.Abs(diff.x), _cameraSize.Height / Mathf.Abs(diff.y));
		}

		public void Update()
		{
			if (_needUpdateInertia)
			{
				UpdateInertia();
			}
		}

		private void OnMouseDown(int entity)
		{
			_needUpdateInertia = false;
			_moveVel = Vector2.zero;
		}

		private void OnMouseMoved(int entity)
		{
			_world.Get(entity, out MouseMoved evt);
			TranslateTarget(evt.pos - evt.lastPos);

			_moveVel = Vector2.Lerp(_moveVel, (evt.pos - evt.lastPos) / Time.deltaTime, Time.deltaTime * 10);
		}

		private void OnMouseUp(int entity)
		{
			_needUpdateInertia = true;
			_moveSpeed = _moveVel.magnitude;
			_moveVel.Normalize();
		}

		private void OnMouseZoomed(int entity)
		{
			_needUpdateInertia = false;
			_moveVel = Vector2.zero;

			_world.Get(entity, out MouseZoomed evt);

			var root = _ui.root;
			var scale = root.localScale.x;
			var newScale = Mathf.Clamp(scale + evt.delta * 0.1f, _minScale, _constants.maxZoomScale);
			var diff = root.localPosition - evt.pos;

			root.localScale = Vector3.one * newScale;
			root.localPosition = evt.pos + diff * (newScale / scale);

			TranslateTarget(Vector2.zero);
		}

		private void TranslateTarget(Vector2 diff)
		{
			Vector2 topLeft = _ui.rootTL.position;
			Vector2 botRight = _ui.rootBR.position;

			topLeft += diff;
			botRight += diff;

			var diffMinX = topLeft.x - _cameraSize.Left;
			if (diffMinX > 0)
			{
				diff.x -= diffMinX;
			}

			var diffMaxX = botRight.x - _cameraSize.Right;
			if (diffMaxX < 0)
			{
				diff.x -= diffMaxX;
			}

			var diffMinY = botRight.y - _cameraSize.Bottom;
			if (diffMinY > 0)
			{
				diff.y -= diffMinY;
			}

			var diffMaxY = topLeft.y - _cameraSize.Top;
			if (diffMaxY < 0)
			{
				diff.y -= diffMaxY;
			}

			_ui.root.Translate(diff);
		}

		private void UpdateInertia()
		{
			_moveSpeed *= Mathf.Pow(kDecelerationRate, Time.deltaTime);
			Vector2 deltaPos = _moveVel * _moveSpeed * Time.deltaTime;
			TranslateTarget(deltaPos);

			if (_moveSpeed <= 0.1f)
			{
				_needUpdateInertia = false;
			}
		}
	}
}