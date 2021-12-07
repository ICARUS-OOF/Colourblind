using Colourblind.Managers;
using Colourblind.Movement;
using Unity.VisualScripting;
using UnityEngine;
namespace Colourblind.Objects
{
    public class MovingCogwheel : MonoBehaviour
    {
        [SerializeField] private Vector3 turnDirection;
        [SerializeField] private bool damagePlayer = true;

        private void Update()
        {
            if (GetComponent<Rigidbody>() == null)
                transform.Rotate(turnDirection * TimeManager.GetFixedDeltaTime());
        }

        private void LateUpdate()
        {
            
        }

        public void DropCogwheel()
        {
            Rigidbody rb = transform.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.mass = 3f;
            transform.parent = null;
            AudioManager.PlayProfiledSoundEffect("Metal Fall", transform, .7f, .5f, pitch: 1f);
            CameraOrientation.Instance.Shake(Vector3.one * 2f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody _rb = transform.GetComponent<Rigidbody>();

            if (_rb != null)
            {
                if (_rb.velocity.y < -3f)
                {
                    int layer = collision.gameObject.layer;
                    if (collision.transform.tag == "Player" && damagePlayer)
                    {
                        //Iterate through every collision in a physics update
                        for (int i = 0; i < collision.contactCount; i++)
                        {
                            Vector3 normal = collision.contacts[i].normal;
                            //FLOOR
                            if (IsFloor(normal))
                            {

                            }
                        }
                    } else if (3 != (3 | (1 << layer)))
                    {
                        //Iterate through every collision in a physics update
                        for (int i = 0; i < collision.contactCount; i++)
                        {
                            Vector3 normal = collision.contacts[i].normal;
                            //FLOOR
                            if (IsFloor(normal))
                            {
                                CameraOrientation.Instance.Shake(new Vector3(-_rb.velocity.y * .35f, -_rb.velocity.y * .35f, -_rb.velocity.y * .35f));
                                AudioManager.PlayProfiledSoundEffect("Metal Impact", transform, -_rb.velocity.y * .1f, .5f, pitch: .8f);
                                GameObject _particles = Instantiate(CogwheelManager.Instance.cogwheelSmokeParticles, collision.contacts[i].point, Quaternion.identity);
                                _particles.transform.localScale = new Vector3(-_rb.velocity.y * .2f, -_rb.velocity.y * .2f, -_rb.velocity.y * .2f);
                                Destroy(_particles, 1f);
                            }
                        }
                    }
                }
            }
        }

        private bool IsFloor(Vector3 v)
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < 45;
        }
    }
}