using Colourblind.Systems;
using UnityEngine;

namespace Colourblind.Data
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.tag == "Player")
            {
                Player.Instance.Die();
            }
        }
    }
}