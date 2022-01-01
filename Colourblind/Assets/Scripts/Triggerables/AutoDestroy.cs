using Colourblind.Interfaces;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class AutoDestroy : TriggerableBehaviour
    {
        [SerializeField] private float destoryTime = 15f;
        [SerializeField] private bool selfDestruct = false;

        private void Start()
        {
            if (selfDestruct)
            {
                Destroy(this.gameObject, destoryTime);
            }
        }

        public override void Trigger()
        {
            Destroy(this.gameObject, destoryTime);
        }
    }
}