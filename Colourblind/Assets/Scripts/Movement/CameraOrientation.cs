using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colourblind.Movement
{
    public class CameraOrientation : MonoBehaviour
    {
        public static CameraOrientation Instance;
        public Animator cameraAnimator;

        [SerializeField] private Transform target;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Update()
        {
            transform.position = target.position;
        }
    }
}