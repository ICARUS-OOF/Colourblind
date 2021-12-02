using Colourblind.Managers;
using System;
using Colourblind.Data;
using UnityEngine;

namespace Colourblind.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        //Assignables
        [SerializeField] private PlayerMovementData movementData;

        [SerializeField] private Transform playerCam;
        [SerializeField] private Transform orientation;
        [SerializeField] private CameraOrientation camOri;

        //Other
        [HideInInspector] public Rigidbody rb;

        //DO NOT USE THIS BOOLEAN
        public bool canMove;

        //Rotation and look
        private float xRotation;
        private float sensMultiplier = 1f;

        //Movement
        [SerializeField] private bool grounded;

        private float threshold = 0.01f;

        private float fallDuration;

        //Crouch & Slide
        private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
        private Vector3 playerScale;

        //Jumping
        private bool readyToJump = true;
        private float jumpCooldown = 0.25f;
        private bool jumped = false;

        //Input
        private float x, y;
        private bool jumping, sprinting, crouching;

        //Sliding
        private Vector3 normalVector = Vector3.up;
        private Vector3 wallNormalVector;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            playerScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                Movement();
            }
        }

        private void Update()
        {
            if (canMove)
            {
                FindInputs();
                Look();
                CalculateFallDuration();
            }
        }

        private void LateUpdate()
        {

        }

        /// <summary>
        /// Find user input. Should put this in its own class but im lazy
        /// </summary>
        private void FindInputs()
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            jumping = Input.GetKey(KeyCode.Space);

            /*
            crouching = Input.GetKey(KeyCode.LeftControl);

            //Crouching
            if (Input.GetKeyDown(KeyCode.LeftControl))
                StartCrouch();
            if (Input.GetKeyUp(KeyCode.LeftControl))
                StopCrouch();
            */
        }

        public void AddForce(Vector3 force)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        public void AddForceAtPosition(Vector3 force, Vector3 pos)
        {
            rb.AddForceAtPosition(force, pos, ForceMode.Impulse);
        }

        private void StartCrouch()
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            if (rb.velocity.magnitude > 0.5f)
            {
                if (grounded)
                {
                    rb.AddForce(orientation.transform.forward * movementData.GetSlideForce());
                }
            }
        }

        private void CalculateFallDuration()
        {
            if (grounded)
            {
                if (fallDuration > 0)
                {
                    //feedback.Land(fallDuration);
                }
                fallDuration = 0;
            }
            else
            {
                fallDuration += TimeManager.GetFixedDeltaTime();
            }
        }

        private void StopCrouch()
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

        private void Movement()
        {
            //Extra gravity
            rb.AddForce(Vector3.down * TimeManager.GetFixedDeltaTime() * 1000f);

            //Find actual velocity relative to where player is looking
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            //Counteract sliding and sloppy movement
            CounterMovement(x, y, mag);

            //If holding jump && ready to jump, then jump
            if (readyToJump && jumping) Jump();

            //Set max speed
            float maxSpeed = movementData.GetMoveSpeed();

            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (y > 0 && yMag > maxSpeed) y = 0;
            if (y < 0 && yMag < -maxSpeed) y = 0;

            //Some multipliers
            float multiplier = 1f, multiplierV = 1f;

            // Movement in air
            if (!grounded)
            {
                /*
                if (jumped)
                {
                    multiplier = 0.275f;
                    multiplierV = 0.1f;
                    multiplierX = 0.7f;
                } else
                {
                    multiplier = 0.385f;
                    multiplierV = 0.06f;
                    multiplierX = 0.4f;
                }
                */
                multiplier = 0.5f;
                multiplierV = 0.5f;
            }

            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * y * movementData.GetMoveSpeed() * TimeManager.GetFixedDeltaTime() * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * movementData.GetMoveSpeed() * TimeManager.GetFixedDeltaTime() * multiplier * multiplierV);

            //Fall Reduction
            if (rb.velocity.y < -movementData.GetMaxYSpeed())
            {
                rb.velocity = new Vector3(rb.velocity.x, -movementData.GetMaxYSpeed(), rb.velocity.z);
            }

            //Movement Shaking
            if (GetXZVelocity().magnitude > .01f)
            {
                //MovementShake();
            }

            /*
            //Vaulting
            if (GetXZYDividedVelocity().magnitude > 3f)
            {
                Vaulting();
            }
            */
        }

        public Vector3 GetVelocity()
        {
            return rb.velocity;
        }

        public void SetVelocity(Vector3 vel)
        {
            rb.velocity = vel;
        }

        private void Jump()
        {
            if (grounded && readyToJump)
            {
                readyToJump = false;
                jumped = true;

                //Add jump forces
                rb.AddForce(Vector2.up * movementData.GetJumpForce() * 1.5f);
                rb.AddForce(normalVector * movementData.GetJumpForce() * 0.5f);

                //If jumping while falling, reset y velocity.
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x / 3.8f, 0, vel.z / 3.8f);
                else if (rb.velocity.y > 0)
                    rb.velocity = new Vector3(vel.x / 3.8f, vel.y / 2, vel.z / 3.8f);

                Invoke(nameof(ResetJump), jumpCooldown);
                Invoke(nameof(ResetJumped), 4.5f);
            }
        }

        private void ResetJump()
        {
            readyToJump = true;
        }

        private void ResetJumped()
        {
            jumped = false;
        }

        private float desiredX;
        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * movementData.GetSensitivity() * TimeManager.GetFixedDeltaTime() * sensMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * movementData.GetSensitivity() * TimeManager.GetFixedDeltaTime() * sensMultiplier;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        }

        public void SetXRotation(float _XRotation)
        {
            xRotation = _XRotation;
        }

        public float GetXRotation()
        {
            return xRotation;
        }

        private Vector3 GetXZVelocity()
        {
            return new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        private Vector3 GetXZYDividedVelocity()
        {
            return new Vector3(rb.velocity.x, rb.velocity.y / 2, rb.velocity.z);
        }

        private void CounterMovement(float x, float y, Vector2 mag)
        {
            if (!grounded || jumping) return;

            //Slow down sliding
            if (crouching)
            {
                rb.AddForce(movementData.GetMoveSpeed() * TimeManager.GetFixedDeltaTime() * -rb.velocity.normalized * movementData.GetSlideCounterMovement());
                return;
            }

            //Counter movement
            if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
            {
                rb.AddForce(movementData.GetMoveSpeed() * orientation.transform.right * TimeManager.GetFixedDeltaTime() * -mag.x * movementData.GetCounterMovement());
            }
            if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
            {
                rb.AddForce(movementData.GetMoveSpeed() * orientation.transform.forward * TimeManager.GetFixedDeltaTime() * -mag.y * movementData.GetCounterMovement());
            }

            //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
            if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > movementData.GetMaxSpeed())
            {
                float fallspeed = rb.velocity.y;
                Vector3 n = rb.velocity.normalized * movementData.GetMaxSpeed();
                rb.velocity = new Vector3(n.x, fallspeed, n.z);
            }
        }

        /// <summary>
        /// Find the velocity relative to where the player is looking
        /// Useful for vectors calculations regarding movement and limiting movement
        /// </summary>
        /// <returns></returns>
        public Vector2 FindVelRelativeToLook()
        {
            float lookAngle = orientation.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = rb.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

            return new Vector2(xMag, yMag);
        }

        private bool IsFloor(Vector3 v)
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < movementData.GetMaxSlopeAngle();
        }

        private bool cancellingGrounded;

        /// <summary>
        /// Handle ground detection
        /// </summary>
        private void OnCollisionStay(Collision other)
        {
            //Make sure we are only checking for walkable layers
            int layer = other.gameObject.layer;
            //if (whatIsGround != (whatIsGround | (1 << layer))) { return; }
            //if (whatIsGround2 != (whatIsGround2 | (1 << layer))) { return; }

            if (movementData.GetWhatIsGround() != (movementData.GetWhatIsGround() | (1 << layer)) && movementData.GetWhatIsGround2() != (movementData.GetWhatIsGround2() | (1 << layer)))
            { return; }

            //Iterate through every collision in a physics update
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector3 normal = other.contacts[i].normal;
                //FLOOR
                if (IsFloor(normal))
                {
                    grounded = true;
                    cancellingGrounded = false;
                    normalVector = normal;
                    CancelInvoke(nameof(StopGrounded));
                }
            }

            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded)
            {
                cancellingGrounded = true;
                Invoke(nameof(StopGrounded), TimeManager.GetFixedDeltaTime() * delay);
            }
        }

        private void StopGrounded()
        {
            grounded = false;
        }
    }
}