using UnityEngine;

namespace Colourblind.Systems
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public bool canMove = true;
        public bool canLook = true;
        public bool canConfigureColour = true;
        private Rigidbody rb;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            rb = GetComponent<Rigidbody>();
        }

        public void EnableMovement()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            GetComponent<Collider>().enabled = true;
            canMove = true;
            canLook = true;
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().enabled = false;
        }

        public void DisableMovement(bool disableBody = true, bool disableCollider = true, bool disableLook = false)
        {
            if (disableBody)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            if (disableCollider)
            {
                GetComponent<Collider>().enabled = false;
            }
            if (disableLook)
            {
                canLook = false;
            }
            canMove = false;
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().enabled = true;
        }

        public void Die()
        {
            DisableMovement(disableBody: false, disableCollider: false, disableLook: true);
            PlayerUI.Instance.DeathScreen();
        }
    }
}