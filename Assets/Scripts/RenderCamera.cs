using UnityEngine;

public class RenderCamera : MonoBehaviour
{
	private Camera targetCamera;

	private Camera _camera;

	[SerializeField]
	private TopCamera topCamera;

	private bool show = true;

	public Material blackMaterial;

	private float brightness;

	public float blackFadeSpeed = 3f;

	public Camera TargetCamera => targetCamera;

	public Camera Camera => _camera;

	public TopCamera TopCamera => topCamera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
		topCamera.blackMaterial = blackMaterial;
	}

	private void Update()
	{
		UpdateBlack();
	}

	private void LateUpdate()
	{
		if (targetCamera != null)
		{
			base.transform.parent = targetCamera.transform;
			base.transform.position = targetCamera.transform.position;
			base.transform.rotation = targetCamera.transform.rotation;
			_camera.farClipPlane = targetCamera.farClipPlane;
			_camera.nearClipPlane = TargetCamera.nearClipPlane;
			_camera.cullingMask = TargetCamera.cullingMask;
			_camera.fieldOfView = TargetCamera.fieldOfView;
		}
	}

	public void SetTarget(Camera _target)
	{
		targetCamera = _target;
	}

	public void FadeIn()
	{
		show = true;
	}

	public void FadeOut()
	{
		show = false;
	}

	private void UpdateBlack()
	{
		if (Time.unscaledDeltaTime < 0.1f)
		{
			if (show && brightness < 1f)
			{
				brightness = Mathf.Clamp01(brightness + Time.unscaledDeltaTime * blackFadeSpeed);
			}
			else if (!show && brightness > 0f)
			{
				brightness = Mathf.Clamp01(brightness - Time.unscaledDeltaTime * blackFadeSpeed);
			}
			topCamera.brightness = brightness;
		}
	}
}
