using Colourblind.Interfaces;
using Colourblind.Managers;
using Colourblind.Movement;
using Colourblind.Systems;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class ChainedPlatform : TriggerableBehaviour
    {
        public bool isTriggered = false, pingpong = false, isEnabled = true;
        public Transform triggeredPlatformPoint, originalPlatformPoint;

        private Rigidbody rb;
        [SerializeField] private float moveSpeed = 1.9f;
        [SerializeField] private float objectFollowSpeed = 10f;
        [SerializeField] private AudioSource machineSFX;

        private void Awake()
        {
            rb = transform.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (machineSFX != null)
            {
                machineSFX.Play();
                machineSFX.volume = .07f;
            }
        }

        public override void Trigger()
        {
            isTriggered = true;
        }

        public override void Untrigger()
        {
            isTriggered = false;
        }

        private void Update()
        {

        }

        private bool attachedPlayer;

        private void FixedUpdate()
        {
            /*
            if (attachedPlayer && GetComponent<Animator>() != null)
            {
                Vector3 velocityChange = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
                PlayerMovement.Instance.transform.parent = transform;
                PlayerMovement.Instance.rb.velocity += new Vector3(velocityChange.x * objectFollowSpeed * TimeManager.GetFixedDeltaTime(), 0f, velocityChange.z * objectFollowSpeed * TimeManager.GetFixedDeltaTime());
            }
            */
            if (!isEnabled)
            { return; }

            if (pingpong)
            {
                if (rb.position == triggeredPlatformPoint.position || rb.position == originalPlatformPoint.position)
                {
                    isTriggered = !isTriggered;
                }
            }

            if (isTriggered)
            {
                rb.MovePosition(Vector3.MoveTowards(rb.position, triggeredPlatformPoint.position, TimeManager.GetFixedDeltaTime() * moveSpeed));
            }
            else
            {
                rb.MovePosition(Vector3.MoveTowards(rb.position, originalPlatformPoint.position, TimeManager.GetFixedDeltaTime() * moveSpeed));
            }

            if (rb.velocity.magnitude > .1f)
            {
                machineSFX.volume = Mathf.Lerp(machineSFX.volume, .3f, TimeManager.GetFixedDeltaTime() * 5f);
            } else
            {
                machineSFX.volume = Mathf.Lerp(machineSFX.volume, 0f, TimeManager.GetFixedDeltaTime() * 5f);
            }
        }

        private void OnCollisionStay(Collision col)
        {
            Vector3 direction = -(transform.position - col.gameObject.transform.position);
            Vector3 velocityChange = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (Mathf.Abs(direction.x) <= Mathf.Abs(direction.y))
            {
                if (direction.y > 0)
                {
                    if (col.transform.tag == "Player")
                    {
                        if (isEnabled)
                        {
                            col.transform.parent = transform;
                            PlayerMovement.Instance.rb.velocity += new Vector3(velocityChange.x * objectFollowSpeed * TimeManager.GetFixedDeltaTime(), velocityChange.y, velocityChange.z * objectFollowSpeed * TimeManager.GetFixedDeltaTime());
                        }
                        else if (GetComponent<Animator>() != null)
                        {
                            attachedPlayer = true;
                            Vector3 velocityChangeY = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
                            PlayerMovement.Instance.transform.parent = transform;
                            PlayerMovement.Instance.rb.velocity += new Vector3(velocityChangeY.x * objectFollowSpeed * TimeManager.GetFixedDeltaTime(), 0f, velocityChangeY.z * objectFollowSpeed * TimeManager.GetFixedDeltaTime());
                        }
                    }
                    else if (col.transform.tag == "CoreCube")
                    {
                        if (rb.velocity.magnitude > 0.1f)
                        {
                            col.transform.parent = transform;
                        } else
                        {
                            col.transform.parent = null;
                        }
                    }
                } else if (direction.y < 0 && rb.velocity.y < -.01f)
                {
                    if (col.transform.tag == "Player" && PlayerMovement.Instance.grounded)
                    {

                    }
                }
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (col.transform.tag == "Player")
            {
                attachedPlayer = false;
                col.transform.parent = null;
            }
            else if (col.transform.tag == "CoreCube")
                col.transform.parent = null;
        }
    }
}