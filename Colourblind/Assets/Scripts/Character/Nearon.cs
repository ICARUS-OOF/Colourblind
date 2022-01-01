using UnityEngine;

namespace Colourblind.Movement
{
    public class Nearon : MonoBehaviour
    {
        [SerializeField] private Animator nearonAnimator, nearonEyeAnimator;

        public void PlayAnimation(string _parameter)
        {
            nearonAnimator.SetTrigger(_parameter);
        }

        public void PlayEyeAnimation(string _parameter)
        {
            nearonEyeAnimator.SetTrigger(_parameter);
        }
    }
}