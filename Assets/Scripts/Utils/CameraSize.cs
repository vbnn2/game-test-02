using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
	public class CameraSize : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;


		[SerializeField]
		private List<Transform> _borders;

		private float _thickness;
		private float _width;
		private float _height;

		public float Width => _width;
		public float Height => _height;
		public float Left => -_width / 2;
		public float Right => _width / 2;
		public float Top => _height / 2;
		public float Bottom => -_height / 2;
		public float Ratio => _width / _height;
		public Vector3 TopLeft => transform.position + new Vector3(Left, Top, 0);
		public Vector3 BottomRight => transform.position + new Vector3(Right, Bottom, 0);
		public Camera Camera => _camera;

		private void Awake()
		{
			SetOrthoSize(_camera.orthographicSize);
			Assert.IsTrue(_borders.Count == 4, "Must be 4");
		}

		public void SetOrthoSize(float size)
		{
			_camera.orthographicSize = size;
			_height = _camera.orthographicSize * 2;
			_width = _height * _camera.aspect;

			UpdateBorderTransform();
		}

		public void SetBorderThickness(float thickness)
		{
			_thickness = thickness;
			UpdateBorderTransform();
		}

		private void UpdateBorderTransform()
		{
			// L
			_borders[0].localScale = new Vector3(_thickness, Height, 1);
			_borders[0].localPosition = new Vector3(Left + _thickness / 2, 0, 10);

			// T
			_borders[1].localScale = new Vector3(Width, _thickness, 1);
			_borders[1].localPosition = new Vector3(0, Top - _thickness / 2, 10);

			// B
			_borders[2].localScale = new Vector3(Width, _thickness, 1);
			_borders[2].localPosition = new Vector3(0, Bottom + _thickness / 2, 10);

			// R
			_borders[3].localScale = new Vector3(_thickness, Height, 1);
			_borders[3].localPosition = new Vector3(Right - _thickness / 2, 0, 10);
		}
	}
}