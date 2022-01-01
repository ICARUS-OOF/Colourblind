using TMPro;
using UnityEngine;

namespace Colourblind.Managers
{
    public class SaveBlock : MonoBehaviour
    {
        public GameObject arrow;
        [SerializeField] private TMP_Text saveDate;

        public void FillSaveDate(string _saveDate)
        {
            saveDate.text = "[" +  _saveDate + "]";
        }

        public void EmptySlot()
        {
            saveDate.text = "";
        }
    }
}