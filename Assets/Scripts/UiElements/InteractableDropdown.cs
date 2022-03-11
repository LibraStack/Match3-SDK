using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UiElements
{
    public class InteractableDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        
        public int SelectedIndex => _dropdown.value;

        public void AddItems(IEnumerable<string> items)
        {
            _dropdown.AddOptions(items.ToList());
        }
    }
}