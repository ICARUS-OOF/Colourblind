using Colourblind.Interfaces;
using Colourblind.Managers;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class SceneTransitionZone : TriggerableBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.tag == "Player")
            {
                GameManager.onLoadScene?.Invoke();
            }
        }
    }
}