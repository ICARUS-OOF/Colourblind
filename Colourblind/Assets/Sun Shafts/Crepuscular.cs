using Colourblind.Managers;
using Colourblind.Movement;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Crepuscular Rays", -1)]
public class Crepuscular : MonoBehaviour
{
	public Material material;
	public GameObject light;

	// Start is called before the first frame update
	void Start()
	{
		material.SetFloat("Weight", 0f);
		if (light == null)
		{
			light = FindObjectOfType<Light>().gameObject;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (light != null)
		{
			RaycastHit _hit;
			if (Physics.Raycast(light.transform.position, PlayerMovement.Instance.transform.position - light.transform.position, out _hit, 5000f, ~6))
			{
				if (_hit.transform.tag == "Player")
				{
					//WorldToViewportPoint
					material.SetFloat("Weight", Mathf.Lerp(material.GetFloat("Weight"), 1f, TimeManager.GetFixedDeltaTime() * 1.3f));
					material.SetVector("_LightPos", GetComponent<Camera>().WorldToViewportPoint(transform.position - light.transform.forward));
				} else
				{
					material.SetFloat("Weight", Mathf.Lerp(material.GetFloat("Weight"), 0f, TimeManager.GetFixedDeltaTime() * 1.3f));
				}
			} else
            {
				material.SetFloat("Weight", Mathf.Lerp(material.GetFloat("Weight"), 0f, TimeManager.GetFixedDeltaTime() * 1.3f));
			}
		} else
		{
			material.SetFloat("Weight", Mathf.Lerp(material.GetFloat("Weight"), 0f, TimeManager.GetFixedDeltaTime() * 1.3f));
		}

		Graphics.Blit(source, destination, material);
	}
}
