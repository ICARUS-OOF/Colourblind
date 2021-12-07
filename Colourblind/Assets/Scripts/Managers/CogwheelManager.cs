using Colourblind.Interfaces;
using Colourblind.Managers;
using Colourblind.Objects;
using UnityEngine;

namespace Colourblind.Movement
{
    public class CogwheelManager : TriggerableBehaviour
    {
        public static CogwheelManager Instance;

        public MovingCogwheel[] movingCogwheels;
        public GameObject[] objsToDestroy, objsToDestroy2;
        public Rigidbody[] rbToEnable;
        public Transform[] explosionPoints;
        public GameObject cogwheelSmokeParticles;

        public float explosionCogwheelRadius = 30f, explosionCogwheelForce = 600f;

        private int index = 0, index2 = 0;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public override void Trigger()
        {
            DropCogwheel();
        }

        public void DropCogwheel()
        {
            movingCogwheels[index].DropCogwheel();
            index++;
        }

        private int rbIndex;
        public void EnableRB()
        {
            rbToEnable[rbIndex].isKinematic = false;
            rbToEnable[rbIndex].useGravity = true;
            rbIndex++;
        }

        public void ExplodeCogwheel()
        {
            Collider[] colliders = Physics.OverlapSphere(explosionPoints[index2].position, explosionCogwheelRadius, ~3);

            foreach (Collider explodable in colliders)
            {
                Rigidbody rb = explodable.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    AudioManager.PlayProfiledSoundEffect("Metal Fall", explodable.transform, 2f, .5f, pitch: 1f);
                    rb.AddExplosionForce(explosionCogwheelForce, explosionPoints[index2].position, explosionCogwheelRadius);
                }
            }

            index2++;
        }

        public void DestroyObjs()
        {
            for (int i = 0; i < objsToDestroy.Length; i++)
            {
                Destroy(objsToDestroy[i]);
            }
        }

        public void DestroyObjs2()
        {
            for (int i = 0; i < objsToDestroy2.Length; i++)
            {
                Destroy(objsToDestroy2[i]);
            }
        }
    }
}