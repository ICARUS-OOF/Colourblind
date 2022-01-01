using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Auto Crepuscular Rays", -1)]
public class AutoSunRay : MonoBehaviour
{
	public Material material;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("Weight", 1f);
		Graphics.Blit(source, destination, material);
	}
}
