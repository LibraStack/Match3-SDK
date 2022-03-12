using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UiElements
{
    public class InteractableButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event UnityAction Click
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }
    }
}