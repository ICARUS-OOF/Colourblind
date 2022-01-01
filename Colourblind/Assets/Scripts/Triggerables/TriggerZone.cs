using Colourblind.Interfaces;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class TriggerZone : TriggerableBehaviour
    {
        public bool isTriggered = false;

        [SerializeField] private int maxTriggers = 1;
        private int currentTriggers = 0;

        public TriggerableBehaviour[] triggerables;
        public GameObject[] objsToEnable, objsToDisable;

        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.tag == "Player")
            {
                Trigger();
            }
        }

        public override void Trigger()
        {
            if (maxTriggers <= currentTriggers)
            { return; }

            if (currentTriggers > maxTriggers)
            {
                currentTriggers = maxTriggers;
            }

            currentTriggers++;

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
            }
            for (int i = 0; i < objsToEnable.Length; i++)
            {
                objsToEnable[i].SetActive(true);
            }
            for (int i = 0; i < objsToDisable.Length; i++)
            {
                objsToDisable[i].SetActive(false);
            }
        }

        public override void Untrigger()
        {

        }
    }
}