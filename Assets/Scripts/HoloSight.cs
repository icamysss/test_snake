using UnityEngine;

[ExecuteInEditMode]
public class HoloSight : MonoBehaviour
{
	public Transform sightMarker;

	public Renderer rendererHS;

	private MaterialPropertyBlock prop;

	[ExecuteInEditMode]
	private void Update()
	{
		if (prop == null)
		{
			prop = new MaterialPropertyBlock();
		}
		if ((bool)sightMarker && (bool)rendererHS)
		{
			prop.SetVector("_SightForward", sightMarker.forward);
			prop.SetVector("_SightUp", sightMarker.up);
			prop.SetVector("_SightRight", sightMarker.right);
			prop.SetVector("_SightCenterPos", sightMarker.position);
			rendererHS.SetPropertyBlock(prop);
		}
	}
}
