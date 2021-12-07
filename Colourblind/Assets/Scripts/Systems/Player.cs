using UnityEngine;

namespace Colourblind.Systems
{
    public class Player : MonoBehaviour
    {
        public bool canMove = true;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void EnableMovement()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            GetComponent<Collider>().enabled = true;
            canMove = true;
        }

        public void DisableMovement()
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;
            canMove = false;
        }
    }
}