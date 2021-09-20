using UnityEngine;

namespace Game
{
	public class CameraSize : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;

		private float _width;
		private float _height;

		public float Width => _width;
		public float Height => _height;
		public float Left => -_width / 2;
		public float Right => _width / 2;
		public float Top => _height / 2;
		public float Bottom => -_height / 2;
		public float Ratio => _width / _height;
		public Camera Camera => _camera;

		private void Awake()
		{
			var screenRatio = _camera.aspect;
			_width = _camera.orthographicSize * 2 * screenRatio;
			_height = _camera.orthographicSize * 2;
		}
	}
}