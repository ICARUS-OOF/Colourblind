//Never gonna give you up
using Colourblind.Managers;
using Colourblind.Systems;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Colourblind.Movement
{
    public class CameraOrientation : MonoBehaviour
    {
        public static CameraOrientation Instance;
        public Animator cameraAnimator;

        [SerializeField] private PostProcessVolume postProcessEffects;
        [SerializeField] private Player player;
        [SerializeField] private Transform target;

        private Vector3 currentRecoilRotation;
        private Vector3 Rot;

        [Header("Recoil")]
        [SerializeField] private Transform recoilTransform;

        [SerializeField] private float returnSpeed = 25f;
        [SerializeField] private float recoilRotationSpeed = 25f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            postProcessEffects.weight = Mathf.Clamp(GameManager.Instance.currentSettingsData.gfx, 0.5f, 1f);

            if (player.canMove)
            {
                transform.position = target.position;
            }
            else
                transform.position = Vector3.Lerp(transform.position, target.position, TimeManager.GetFixedDeltaTime() * 2f);
        }

        private void FixedUpdate()
        {
            currentRecoilRotation = Vector3.Lerp(currentRecoilRotation, Vector3.zero, returnSpeed * TimeManager.GetFixedDeltaTime());
            Rot = Vector3.Slerp(Rot, currentRecoilRotation, recoilRotationSpeed * TimeManager.GetFixedDeltaTime());
            recoilTransform.localRotation = Quaternion.Euler(Rot);
        }

        public void Shake(Vector3 recoil)
        {
            currentRecoilRotation += new Vector3(-recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
        }

        public void Shake(float recoil)
        {
            currentRecoilRotation += new Vector3(-recoil, Random.Range(-recoil, recoil), Random.Range(-recoil, recoil));
        }
    }
}