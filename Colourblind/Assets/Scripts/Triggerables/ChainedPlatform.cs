using Colourblind.Interfaces;
using Colourblind.Managers;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class ChainedPlatform : TriggerableBehaviour
    {
        public bool isTriggered = false;
        [SerializeField] private Transform triggeredPlatformPoint, originalPlatformPoint;

        public override void Trigger()
        {
            isTriggered = true;
        }

        public override void Untrigger()
        {
            isTriggered = false;
        }

        private void Update()
        {
            if (isTriggered)
            {
                transform.position = Vector3.MoveTowards(transform.position, triggeredPlatformPoint.position, TimeManager.GetFixedDeltaTime() * 3f);
            } else
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPlatformPoint.position, TimeManager.GetFixedDeltaTime() * 3f);
            }
        }
    }
}