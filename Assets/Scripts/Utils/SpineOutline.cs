using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SpineOutline : MonoBehaviour
{
	[SerializeField]
	private int _textureSize = 512;

	[SerializeField]
	private Vector3 _meshPosition = new Vector3(0.5f, 0.5f, -1);
	
	[SerializeField]
	private float _meshScale = 0.4f;

	private MeshFilter _meshFilter;
	private MeshRenderer _meshRenderer;
	private RenderTexture _rt;
	private Texture2D _texture;
	private Matrix4x4 _objectMatrix;
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rt = RenderTexture.GetTemporary(_textureSize, _textureSize, 0, GraphicsFormat.R8G8B8A8_UNorm);
		_texture = new Texture2D(_textureSize, _textureSize, GraphicsFormat.R8G8B8A8_UNorm, 0);
		_spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
	}

	public void Active()
	{
		_meshFilter = GetComponentInParent<MeshFilter>();
		_meshRenderer = GetComponentInParent<MeshRenderer>();
	}

	void Update()
	{
		// Create the object transform matrix
		_objectMatrix = Matrix4x4.TRS(_meshPosition, Quaternion.identity, _meshFilter.transform.localScale * _meshScale);
	}

	void OnWillRenderObject()
	{
		// Create an orthographic matrix (for 2D rendering)
		// You can otherwise use Matrix4x4.Perspective()
		Matrix4x4 projectionMatrix = Matrix4x4.Ortho(0, 1, 0, 1, 0.1f, 100);

		if (Camera.current != null)
			projectionMatrix *= Camera.current.worldToCameraMatrix.inverse;

		RenderTexture prevRT = RenderTexture.active;
		RenderTexture.active = _rt;

		_meshRenderer.material.SetPass(0);

		// Push the projection matrix
		GL.PushMatrix();
		GL.LoadProjectionMatrix(projectionMatrix);

		// It seems that the faces are in a wrong order, so we need to flip them
		GL.invertCulling = true;

		// Clear the texture
		GL.Clear(true, true, Color.clear);

		// Draw the mesh!
		Graphics.DrawMeshNow(_meshFilter.mesh, _objectMatrix);

		// Pop the projection matrix to set it back to the previous one
		GL.PopMatrix();

		// Revert culling
		GL.invertCulling = false;

		_texture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
		_texture.Apply();
		

		// Re-set the RenderTexture to the last used one
		RenderTexture.active = prevRT;
	}
}
