using Colourblind.Interfaces;
using Colourblind.Managers;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class ButtonTrigger : TriggerableBehaviour
    {
        public TriggerableBehaviour[] triggerables, triggerables2;
        public GameObject[] objsToActivate, objsToDisable;
        public bool timed = false;
        public float timer = 1f, timer2 = 0f;
        public Transform originalPoint, endPoint;

        [SerializeField] private AudioSource buttonAudio;

        private bool isTriggered = false;

        private void Update()
        {
            if (isTriggered)
                transform.position = Vector3.Lerp(transform.position, endPoint.position, TimeManager.GetFixedDeltaTime() * 1f);
            else
                transform.position = Vector3.Lerp(transform.position, originalPoint.position, TimeManager.GetFixedDeltaTime() * 1f);
        }

        public override void Trigger()
        {
            if (isTriggered)
                return;

            isTriggered = true;

            buttonAudio.Play();

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
            }

            for (int i = 0; i < triggerables2.Length; i++)
            {
                triggerables2[i].Trigger();
            }

            for (int i = 0; i < objsToActivate.Length; i++)
            {
                objsToActivate[i].SetActive(true);
            }

            for (int i = 0; i < objsToDisable.Length; i++)
            {
                objsToDisable[i].SetActive(false);
            }

            if (timed)
            { 
                Invoke(nameof(UntriggerButton), timer);
                Invoke(nameof(UntriggerButton2), timer2);
            }
        }

        private void UntriggerButton()
        {
            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Untrigger();
            }
            isTriggered = false;
        }

        private void UntriggerButton2()
        {
            for (int i = 0; i < triggerables2.Length; i++)
            {
                triggerables2[i].Untrigger();
            }
        }
    }
}