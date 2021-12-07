using Colourblind.Interfaces;
using Colourblind.Managers;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Door : TriggerableBehaviour
    {
        [SerializeField] private Transform doorObject, originalPoint, targetPoint;
        public bool isTriggered = false;

        public int requiredTriggers = 1;
        private int currentTriggers = 0;

        public override void Trigger()
        {
            currentTriggers++;

            if (currentTriggers >= requiredTriggers)
            {
                isTriggered = true;
            }
        }

        public override void Untrigger()
        {
            currentTriggers--;

            if (currentTriggers < requiredTriggers)
            {
                isTriggered = false;
            }
        }

        private void Update()
        {
            if (isTriggered)
            {
                if (doorObject.position != targetPoint.position)
                    doorObject.position = Vector3.Lerp(doorObject.position, targetPoint.position, TimeManager.GetFixedDeltaTime() * 6f);
            } else
            {
                if (doorObject.position != originalPoint.position)
                    doorObject.position = Vector3.Lerp(doorObject.position, originalPoint.position, TimeManager.GetFixedDeltaTime() * 6f);
            }
        }
    }
}