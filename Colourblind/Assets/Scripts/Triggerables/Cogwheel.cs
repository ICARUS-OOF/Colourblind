using Colourblind.Enums;
using Colourblind.Interfaces;
using Colourblind.Managers;
using Colourblind.Tools;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Cogwheel : TriggerableBehaviour
    {
        public CoreColour _colour;

        [SerializeField] private Vector3 rotationAxis;
        public Transform cogwheelTransform, cubePoint, followPoint;

        public TriggerableBehaviour[] triggerables;
        public ChainedPlatform[] enablePlatforms;

        public bool isTriggered;

        private void Start()
        {
            if (isTriggered)
            {
                AutoTrigger();
            }
        }

        public override void Trigger()
        {
            isTriggered = true;
            Instantiate(GameManager.GetHeldCube(_colour), cubePoint);
            AudioManager.PlaySoundEffect("Activated");

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
            }

            for (int i = 0; i < enablePlatforms.Length; i++)
            {
                enablePlatforms[i].isEnabled = true;
            }
        }

        private void AutoTrigger()
        {
            Instantiate(GameManager.GetHeldCube(_colour), cubePoint);

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
            }

            for (int i = 0; i < enablePlatforms.Length; i++)
            {
                enablePlatforms[i].isEnabled = true;
            }
        }

        public override void Untrigger()
        {
            isTriggered = false;

            foreach (Transform t in cubePoint)
            {
                Destroy(t.gameObject);
            }

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Untrigger();
            }

            for (int i = 0; i < enablePlatforms.Length; i++)
            {
                enablePlatforms[i].isEnabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (followPoint != null)
                cogwheelTransform.position = followPoint.position;

            if (isTriggered)
            {
                cogwheelTransform.Rotate(-rotationAxis * TimeManager.GetFixedDeltaTime());
            } else
            {
                cogwheelTransform.Rotate(rotationAxis * TimeManager.GetFixedDeltaTime());
            }
        }
    }
}