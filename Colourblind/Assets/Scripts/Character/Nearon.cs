using UnityEngine;

namespace Colourblind.Movement
{
    public class Nearon : MonoBehaviour
    {
        [SerializeField] private Animator nearonEyeAnimator;

        public void PlayEyeAnimation(string _parameter)
        {
            nearonEyeAnimator.SetTrigger(_parameter);
        }
    }
}