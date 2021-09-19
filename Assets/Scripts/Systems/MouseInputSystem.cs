using ECS;
using UnityEngine;

namespace Game
{
	public class MouseInputSystem : ComponentSystem, IInitialize, IUpdate
	{
		private bool _isTouchBegan;
		private Vector3 _lastMousePos;
		private Camera _camera;

		public void Initialize()
		{
			_camera = Camera.main;
		}

		public void Update()
		{
			if (Input.GetMouseButton(0))
			{
				var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

				if (!_isTouchBegan)
				{
					_isTouchBegan = true;
					_lastMousePos = mousePos;
				}
				else
				{
					// _world.CreateEntity(new ShipMoveUnit { value = mousePos - _lastMousePos }, new DestroyEntity());
					_lastMousePos = mousePos;
				}
			}
			else
			{
				_isTouchBegan = false;
			}
		}
	}
}