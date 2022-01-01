using Colourblind.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Scaffolding : TriggerableBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private Vector3 startForce;
        [SerializeField] private TriggerableBehaviour[] triggerables;

        public override void Trigger()
        {
            rb = transform.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.useGravity = true;
            rb.mass = .5f;

            for (int i = 0; i < triggerables.Length; i++)
            {
                triggerables[i].Trigger();
            }

            rb.AddForce(startForce);
            //rb.AddTorque(startForce);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.transform.tag == "DestructionCube" && col.transform.GetComponent<Rigidbody>() == null && rb != null)
            {
                col.transform.AddComponent<Rigidbody>();
                col.transform.localScale = new Vector3(0.93f, 0.93f, 0.93f);
                col.transform.GetComponent<Rigidbody>().mass = .5f;
                col.transform.GetComponent<Rigidbody>().useGravity = true;
                col.transform.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                col.transform.GetComponent<Rigidbody>().AddForce(startForce * .1f);
                col.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 5f);

                if (col.transform.GetComponent<AutoDestroy>() != null)
                {
                    col.transform.GetComponent<AutoDestroy>().Trigger();
                }
            }
        }
    }
}