using UnityEngine;

namespace Colourblind.Data
{
    [CreateAssetMenu(fileName = "New PlayerMovementData", menuName = "Colourblind/Player Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        [SerializeField] private float sensitivity = 85f;

        [SerializeField] private float moveSpeed = 2750f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float maxY = 20f;

        [SerializeField] private LayerMask whatIsGround, whatIsGround2;

        [SerializeField] private float counterMovement = 0.175f;
        [SerializeField] private float maxSlopeAngle = 35f;

        [SerializeField] private float slideForce = 400;
        [SerializeField] private float slideCounterMovement = 100f;

        [SerializeField] private float jumpForce = 285f;


        public float GetSensitivity() { return sensitivity; }

        public float GetMoveSpeed() { return moveSpeed; }
        public float GetMaxSpeed() { return maxSpeed; }
        public float GetMaxYSpeed() { return maxY; }

        public LayerMask GetWhatIsGround() { return whatIsGround; }
        public LayerMask GetWhatIsGround2() { return whatIsGround2; }

        public float GetCounterMovement() { return counterMovement; }
        public float GetMaxSlopeAngle() { return maxSlopeAngle; }

        public float GetSlideForce() { return slideForce; }
        public float GetSlideCounterMovement() { return slideCounterMovement; }

        public float GetJumpForce() { return jumpForce; }
    }
}