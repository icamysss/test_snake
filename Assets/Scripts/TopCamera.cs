using UnityEngine;

public class TopCamera : MonoBehaviour
{
	[HideInInspector]
	public Material blackMaterial;

	[HideInInspector]
	public float brightness;

	private Camera _camera;

	public Camera Camera => _camera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (blackMaterial != null)
		{
			blackMaterial.SetFloat("_Brightness", brightness);
			Graphics.Blit(source, destination, blackMaterial);
		}
	}
}
