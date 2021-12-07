using UnityEngine;

namespace Colourblind.Data
{
    [System.Serializable]
    public struct SoundEffect
    {
        public string ID;
        public AudioClip clip;
        public float volume;
    }
}