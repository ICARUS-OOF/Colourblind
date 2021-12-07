using Colourblind.Enums;
using UnityEngine;

namespace Colourblind.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject[] heldCubes;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else
            {
                Destroy(this.gameObject);
            }
        }

        public static GameObject GetHeldCube(CoreColour _colour)
        {
            int i = ((int)_colour) - 1;

            if (i >= 0)
                return Instance.heldCubes[i];
            else
                return null;
        }
    }
}