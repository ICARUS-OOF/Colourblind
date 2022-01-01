using Colourblind.Interfaces;
using Colourblind.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Pusher : TriggerableBehaviour
    {
        public bool isTriggered = false;
        public int requiredTriggers = 1;
        [SerializeField] private ParticleSystem pushParticles;
        [SerializeField] private Vector3 pushForce;
        private int currentTriggers;

        [SerializeField] private AudioSource pusherSFX;
        
        public override void Trigger()
        {
            currentTriggers++;
            if (currentTriggers >= requiredTriggers)
            {
                pusherSFX.Play();
                isTriggered = true;
                pushParticles.Play();
            }
        }

        public override void Untrigger()
        {
            currentTriggers--;
            if (currentTriggers < requiredTriggers)
            {
                if (pusherSFX.isPlaying)
                    pusherSFX.Stop();
                isTriggered = false;
            }
        }

        private void OnTriggerStay(Collider col)
        {
            /*
            ChainedPlatform _platform = col.transform.GetComponent<ChainedPlatform>();
            // && allowedPlatforms.Contains(_platform)
            if (_platform != null && isTriggered)
            {
                _platform.pusherPoint = pusherPoint;
            }
            */
            ColourCube cube = col.transform.GetComponent<ColourCube>();
            if (cube != null && isTriggered)
            {
                cube.GetComponent<Rigidbody>().AddForce(pushForce);
            }
        }
    }
}