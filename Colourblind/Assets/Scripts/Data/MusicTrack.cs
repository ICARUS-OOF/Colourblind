using UnityEngine;

namespace Colourblind.Data
{
    [System.Serializable]
    public class MusicTrack
    {
        public AudioClip _audioClip, _queuedLoopClip;
        public float volume;
        public bool loop = true, exitUnload = true, fadeOut = false;
    }
}