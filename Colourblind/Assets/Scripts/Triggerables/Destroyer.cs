using Colourblind.Interfaces;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Destroyer : TriggerableBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.tag != "Player")
            {
                Destroy(col.gameObject);
            }
        }
    }
}