using Colourblind.Interfaces;
using Colourblind.Triggerables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colourblind.Managers
{
    public class FactoryManager : MonoBehaviour
    {
        [SerializeField] private TriggerableBehaviour[] triggerables;
        private int currentTriggerIndex = 0;

        public void NextTrigger()
        {
            triggerables[currentTriggerIndex].Trigger();
            currentTriggerIndex++;
        }
    }
}