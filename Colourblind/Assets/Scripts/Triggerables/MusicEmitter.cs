using Colourblind.Data;
using Colourblind.Managers;
using Colourblind.Interfaces;

namespace Colourblind.Triggerables
{
    public class MusicEmitter : TriggerableBehaviour
    {
        public MusicTrack _track;
        public bool autoPlay = true;
        public float delay = 0f;

        private void Start()
        {
            if (autoPlay)
            {
                Invoke(nameof(Trigger), delay);
            }
        }

        public override void Trigger()
        {
            MusicManager.singleton.PlayTrack(_track);
        }
    }
}