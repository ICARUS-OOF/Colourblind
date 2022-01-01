using Colourblind.Interfaces;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class AudioTrigger : TriggerableBehaviour
    {
        private AudioSource sfxSource;

        private void Awake()
        {
            sfxSource = GetComponent<AudioSource>();
        }

        public override void Trigger()
        {
            if (sfxSource != null)
            {
                sfxSource.Play();
            }
        }
    }
}