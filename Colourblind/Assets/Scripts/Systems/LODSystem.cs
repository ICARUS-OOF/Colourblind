using UnityEngine;
using UnityEngine.ProBuilder;

namespace Colourblind.Systems
{
    public class LODSystem : MonoBehaviour
    {
        private MeshFilter meshFilter;
        [SerializeField] private float LOD1Dist = 8f, LOD2Dist = 18f, LOD3Dist = 32f, CullDist = 56f;
        [SerializeField] private Mesh LOD0, LOD1, LOD2, LOD3;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;

            meshFilter = GetComponent<MeshFilter>();

            if (LOD0 == null)
            {
                if (meshFilter != null)
                    LOD0 = meshFilter.mesh;
            }

            InvokeRepeating(nameof(UpdateMesh), 0f, .3f);
        }

        private void UpdateMesh()
        {
            if (cam != null)
            {
                float dist = Vector3.Distance(transform.position, cam.transform.position);
                if (dist >= LOD1Dist)
                {
                    if (dist >= LOD2Dist)
                    {
                        if (dist >= LOD3Dist)
                        {
                            if (dist >= CullDist)
                            {
                                meshFilter.mesh = null;
                            } else
                            {
                                meshFilter.mesh = LOD3;
                            }
                        } else
                        {
                            meshFilter.mesh = LOD2;
                        }
                    } else
                    {
                        meshFilter.mesh = LOD1;
                    }
                } else
                {
                    meshFilter.mesh = LOD0;
                }
            }
        }
    }
}