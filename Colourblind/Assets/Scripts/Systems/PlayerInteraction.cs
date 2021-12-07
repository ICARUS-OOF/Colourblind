using Colourblind.Enums;
using Colourblind.Managers;
using Colourblind.Objects;
using Colourblind.Triggerables;
using UnityEngine;

namespace Colourblind.Systems
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        public float interactRange = 2.5f;

        [SerializeField] private Transform item1Holder, item2Holder;
        [SerializeField] private GameObject item1, item2;

        public CoreColour color1 = CoreColour.None, color2 = CoreColour.None;

        private int currentItemIndex = 1;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(origin.position, origin.forward, out _hitInfo, interactRange))
                {
                    ColourCube _cube = _hitInfo.transform.GetComponent<ColourCube>();
                    if (_cube != null && _cube.canInteract)
                    {
                        if (color1 == CoreColour.None)
                        {
                            //Can put in item slot 1
                            color1 = _cube._colour;
                            item1 = Instantiate(GameManager.GetHeldCube(color1), item1Holder);

                            _cube.Interact();
                        } else if (color2 == CoreColour.None)
                        {
                            //Put in item slot 2
                            color2 = _cube._colour;
                            item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);

                            _cube.Interact();
                        }
                    }
                }
            } else if (Input.GetKeyDown(KeyCode.F))
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(origin.position, origin.forward, out _hitInfo, interactRange))
                {
                    Cogwheel _cogwheel = _hitInfo.transform.GetComponent<Cogwheel>();
                    if (_cogwheel != null && !_cogwheel.isTriggered)
                    {
                        if (currentItemIndex == 1)
                        {
                            if (_cogwheel._colour == color1 && item1 != null)
                            {
                                Destroy(item1);
                                _cogwheel.Trigger();
                            }
                        } else if (currentItemIndex == 2)
                        {
                            if (_cogwheel._colour == color2 && item2 != null)
                            {
                                Destroy(item2);
                                _cogwheel.Trigger(); 
                            }
                        }
                    }
                }
            }
        }
    }
}