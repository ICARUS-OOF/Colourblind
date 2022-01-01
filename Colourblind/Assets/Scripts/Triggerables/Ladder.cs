using Colourblind.Movement;
using UnityEngine;

namespace Colourblind.Triggerables
{
    public class Ladder : MonoBehaviour
    {
        private void OnTriggerStay(Collider col)
        {
            if (col.transform.tag == "Player")
            {
                PlayerMovement.Instance.isLadder = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.transform.tag == "Player")
            {
                PlayerMovement.Instance.isLadder = false;
            }
        }
    }
}