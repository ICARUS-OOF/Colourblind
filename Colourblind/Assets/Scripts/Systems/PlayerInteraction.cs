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
        [SerializeField] private Player player;
        public float interactRange = 2.5f;

        [SerializeField] private Transform item1Holder, item2Holder;
        [SerializeField] private GameObject item1, item2;

        public CoreColour color1 = CoreColour.None, color2 = CoreColour.None;

        private int currentItemIndex = 1;

        private void Update()
        {
            if (PlayerUI.Instance.isPaused)
            { return; }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                if (currentItemIndex == 1)
                {
                    currentItemIndex = 2;
                }
                else if (currentItemIndex == 2)
                {
                    currentItemIndex = 1;
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentItemIndex = 1;
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentItemIndex = 2;
            }

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
                        }
                        else if (color2 == CoreColour.None)
                        {
                            //Put in item slot 2
                            color2 = _cube._colour;
                            item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);

                            _cube.Interact();
                        }
                    }

                    Cogwheel _cogwheel = _hitInfo.transform.GetComponent<Cogwheel>();
                    if (_cogwheel != null)
                    {
                        if (!_cogwheel.isTriggered)
                        {
                            if (currentItemIndex == 1)
                            {
                                if (_cogwheel._colour == color1 && item1 != null)
                                {
                                    color1 = CoreColour.None;
                                    Destroy(item1);
                                    _cogwheel.Trigger();
                                }
                            }
                            else if (currentItemIndex == 2)
                            {
                                if (_cogwheel._colour == color2 && item2 != null)
                                {
                                    color2 = CoreColour.None;
                                    Destroy(item2);
                                    _cogwheel.Trigger();
                                }
                            }
                        }
                        else if (_cogwheel.isTriggered)
                        {
                            if (currentItemIndex == 1)
                            {
                                if (color1 == CoreColour.None)
                                {
                                    //Can put in item slot 1
                                    color1 = _cogwheel._colour;
                                    item1 = Instantiate(GameManager.GetHeldCube(color1), item1Holder);

                                    _cogwheel.Untrigger();
                                }
                            }
                            else if (currentItemIndex == 2)
                            {
                                if (color2 == CoreColour.None)
                                {
                                    //Put in item slot 2
                                    color2 = _cogwheel._colour;
                                    item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);

                                    _cogwheel.Untrigger();
                                }
                            }
                        }
                    }

                    ButtonTrigger _pusher = _hitInfo.transform.GetComponent<ButtonTrigger>();
                    if (_pusher != null)
                    {
                        _pusher.Trigger();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(origin.position, origin.forward, out _hitInfo, interactRange))
                {
                    if (currentItemIndex == 1)
                    {
                        if (item1 != null && color1 != CoreColour.None)
                        {
                            Instantiate(GameManager.GetInteractableCube(color1), _hitInfo.point, Quaternion.identity);
                            color1 = CoreColour.None;
                            Destroy(item1);
                        }
                    }
                    else if (currentItemIndex == 2)
                    {
                        if (item2 != null && color2 != CoreColour.None)
                        {
                            Instantiate(GameManager.GetInteractableCube(color2), _hitInfo.point, Quaternion.identity);
                            color2 = CoreColour.None;
                            Destroy(item2);
                        }
                    }
                }
                else
                {
                    if (currentItemIndex == 1)
                    {
                        if (item1 != null && color1 != CoreColour.None)
                        {
                            Instantiate(GameManager.GetInteractableCube(color1), origin.position + origin.forward * interactRange, Quaternion.identity);
                            color1 = CoreColour.None;
                            Destroy(item1);
                        }
                    }
                    else if (currentItemIndex == 2)
                    {
                        if (item2 != null && color2 != CoreColour.None)
                        {
                            Instantiate(GameManager.GetInteractableCube(color2), origin.position + origin.forward * interactRange, Quaternion.identity);
                            color2 = CoreColour.None;
                            Destroy(item2);
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                if (player.canConfigureColour)
                {
                    if (color1 != CoreColour.None && color2 != CoreColour.None)
                    {
                        CoreColour combinedColor = GameManager.GetCombinedCube(color1, color2);
                        if (combinedColor != CoreColour.None)
                        {
                            if (item1 != null)
                            {
                                Destroy(item1);
                            }

                            if (item2 != null)
                            {
                                Destroy(item2);
                            }

                            if (currentItemIndex == 1)
                            {
                                color1 = combinedColor;
                                item1 = Instantiate(GameManager.GetHeldCube(color1), item1Holder);
                                color2 = CoreColour.None;
                            }
                            else if (currentItemIndex == 2)
                            {
                                color1 = CoreColour.None;
                                color2 = combinedColor;
                                item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);
                            }
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (player.canConfigureColour)
                {
                    if (currentItemIndex == 1)
                    {
                        if (color1 != CoreColour.None && color2 == CoreColour.None)
                        {
                            CoreColour[] disassembledColours = GameManager.GetDisassembledCubes(color1);
                            if (GameManager.CheckDissasembledCubeIsValid(disassembledColours))
                            {
                                if (item1 != null)
                                {
                                    Destroy(item1);
                                }

                                if (item2 != null)
                                {
                                    Destroy(item2);
                                }

                                color1 = disassembledColours[0];
                                Debug.Log("Dissasembled 1: " + color1);
                                item1 = Instantiate(GameManager.GetHeldCube(color1), item1Holder);

                                color2 = disassembledColours[1];
                                Debug.Log("Dissasembled 2: " + color2);
                                item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);
                            }
                        }
                    }
                    else if (currentItemIndex == 2)
                    {
                        if (color2 != CoreColour.None && color1 == CoreColour.None)
                        {
                            CoreColour[] disassembledColours = GameManager.GetDisassembledCubes(color2);
                            if (GameManager.CheckDissasembledCubeIsValid(disassembledColours))
                            {
                                if (item1 != null)
                                {
                                    Destroy(item1);
                                }

                                if (item2 != null)
                                {
                                    Destroy(item2);
                                }

                                color1 = disassembledColours[0];
                                Debug.Log("Dissasembled 1: " + color1);
                                Debug.Log(color1);
                                item1 = Instantiate(GameManager.GetHeldCube(color1), item1Holder);

                                color2 = disassembledColours[1];
                                Debug.Log("Dissasembled 2: " + color2);
                                item2 = Instantiate(GameManager.GetHeldCube(color2), item2Holder);
                            }
                        }
                    }
                } 
            }

            item1Holder.localPosition = Vector3.Lerp(item1Holder.localPosition, currentItemIndex == 1 ? 
                new Vector3(item1Holder.localPosition.x, -.25f, item1Holder.localPosition.z) : new Vector3(item1Holder.localPosition.x, -.35f, item1Holder.localPosition.z),
                TimeManager.GetFixedDeltaTime() * 2.5f);

            item2Holder.localPosition = Vector3.Lerp(item2Holder.localPosition, currentItemIndex == 2 ?
                new Vector3(item2Holder.localPosition.x, -.25f, item2Holder.localPosition.z) : new Vector3(item2Holder.localPosition.x, -.35f, item2Holder.localPosition.z),
                TimeManager.GetFixedDeltaTime() * 2.5f);
        }
    }
}