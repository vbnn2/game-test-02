using ECS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
	public class MouseInputSystem : ComponentSystem, IUpdate
	{
		private bool _isTouching;
		private Vector2 _startPos;
		private Vector2 _lastPos;
		private CameraSize _cameraSize;

		public void Update()
		{
			if (!_isTouching && IsOverUIElement())
				return;

			Vector2 mousePos = _cameraSize.Camera.ScreenToWorldPoint(Input.mousePosition) - _cameraSize.Camera.transform.position;
			if (Input.GetMouseButtonDown(0))
			{
				MouseDown(mousePos);
				return;
			}

			if (Input.GetMouseButtonUp(0))
			{
				MouseUp(mousePos);
				return;
			}

			if (_isTouching)
			{
				MouseMoved(mousePos);
			}

			if (!IsOverUIElement() && Input.mouseScrollDelta.y != 0)
			{
				MouseZoomed(mousePos, Input.mouseScrollDelta.y);
			}
		}

		private void MouseDown(Vector2 mousePos)
		{
			if (!_isTouching)
			{
				_isTouching = true;
			}

			_world.CreateEntity(
				new MouseDown 
				{ 
					pos = mousePos
				},
				new DestroyEntity()
			);

			_lastPos = mousePos;
			_startPos = mousePos;
		}

		private void MouseUp(Vector2 mousePos)
		{
			if (!_isTouching)
				return;

			_isTouching = false;
			_world.CreateEntity(
				new MouseUp
				{
					pos = mousePos,
					lastPos = _lastPos,
					startPos = _startPos 
				},
				new DestroyEntity());
		}

		private void MouseMoved(Vector2 mousePos)
		{
			if (!_isTouching)
				return;

			_world.CreateEntity(
				new MouseMoved
				{
					pos = mousePos,
					lastPos = _lastPos,
					startPos = _startPos
				},
				new DestroyEntity()
			);

			_lastPos = mousePos;
		}

		private void MouseZoomed(Vector2 mousePos, float delta)
		{
			_world.CreateEntity(
				new MouseZoomed
				{
					pos = mousePos,
					delta = delta
				}, 
				new DestroyEntity()
			);
		}

		private bool IsOverUIElement()
		{
			if (EventSystem.current?.IsPointerOverGameObject() ?? false)
				return true;
			return false;
		}
	}
}