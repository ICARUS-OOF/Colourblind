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
        public Transform cogwheelTransform, cubePoint;
        
        public TriggerableBehaviour[] triggerables;

        public bool isTriggered { get; set; }

        public override void Trigger()
        {
            isTriggered = true;
            Instantiate(GameManager.GetHeldCube(_colour), cubePoint);
            AudioManager.PlaySoundEffect("Activated");

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
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
                triggerables[i].Trigger();
            }
        }

        private void Update()
        {
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