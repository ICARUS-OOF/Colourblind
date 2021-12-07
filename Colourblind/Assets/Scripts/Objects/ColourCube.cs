using Colourblind.Enums;
using Colourblind.Interfaces;
using Colourblind.Managers;
using UnityEngine;

namespace Colourblind.Objects
{
    public class ColourCube : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool canInteract { get; set; } = true;

        public CoreColour _colour;

        public void Interact()
        {
            AudioManager.PlaySoundEffect("Activated 2");
            Destroy(this.gameObject);
        }
    }
}