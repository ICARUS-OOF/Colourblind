using Colourblind.Interfaces;
using Colourblind.Managers;
using Colourblind.Movement;
using Colourblind.Systems;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class JumpPad : TriggerableBehaviour
    {
        [SerializeField] private ParticleSystem jumpParticles;

        public Vector3 velocity;

        public bool isTriggered = false;
        public int requiredTriggers = 1;
        private int currentTriggers;

        [SerializeField] private AudioSource windSFX;

        private void Start()
        {
            if (isTriggered)
            {
                jumpParticles.Play();
            }

            windSFX.Play();
            windSFX.volume = .2f;
        }

        private void FixedUpdate()
        {
            if (isTriggered)
            {
                windSFX.volume = Mathf.Lerp(windSFX.volume, .2f, TimeManager.GetFixedDeltaTime() * 5f);
            }
            else
            {
                windSFX.volume = Mathf.Lerp(windSFX.volume, 0f, TimeManager.GetFixedDeltaTime() * 5f);
            }
        }

        public override void Trigger()
        {
            currentTriggers++;
            if (currentTriggers >= requiredTriggers)
            {
                jumpParticles.Play();
                isTriggered = true;
            }
        }

        public override void Untrigger()
        {
            currentTriggers--;
            if (currentTriggers < requiredTriggers)
            {
                jumpParticles.Stop();
                isTriggered = false;
            }
        }

        private void OnTriggerStay(Collider col)
        {
            if (col.transform.tag == "Player" && isTriggered)
            {
                PlayerMovement.Instance.SetVelocity(velocity);
            }
        }
    }
}