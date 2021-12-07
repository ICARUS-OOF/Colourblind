using UnityEngine;

namespace Colourblind.Interfaces
{
    public interface IInteractable
    {
        public bool canInteract { get; set; }
        public void Interact();
    }
}