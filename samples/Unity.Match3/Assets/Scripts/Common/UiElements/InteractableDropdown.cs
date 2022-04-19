using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UiElements
{
    public class InteractableDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        public int SelectedIndex => _dropdown.value;

        public event UnityAction<int> IndexChanged
        {
            add => _dropdown.onValueChanged.AddListener(value);
            remove => _dropdown.onValueChanged.RemoveListener(value);
        }

        public void AddItems(IEnumerable<string> items)
        {
            _dropdown.AddOptions(items.ToList());
        }
    }
}