using System;
using System.Collections.Generic;
using System.Linq;
using Implementation.Common.Interfaces;
using Implementation.Common.UiElements;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common
{
    public class GameUiCanvas : MonoBehaviour, IGameUiCanvas
    {
        [SerializeField] private AppContext _appContext;
        [SerializeField] private InteractableDropdown _fillStrategyDropdown;
        [SerializeField] private InteractableButton _startGameButton;

        private IEnumerable<string> _boardFillStrategies;

        public int SelectedFillStrategyIndex => _fillStrategyDropdown.SelectedIndex;
        
        public event EventHandler StartGameClick;

        private void Awake()
        {
            _boardFillStrategies = _appContext
                .Resolve<IBoardFillStrategy<IUnityItem>[]>()
                .Select(strategy => strategy.Name);
        }

        private void Start()
        {
            _fillStrategyDropdown.AddItems(_boardFillStrategies);
        }

        private void OnEnable()
        {
            _startGameButton.Click += OnStartGameButtonClick;
        }

        private void OnDisable()
        {
            _startGameButton.Click -= OnStartGameButtonClick;
        }

        private void OnStartGameButtonClick()
        {
            StartGameClick?.Invoke(this, EventArgs.Empty);
        }
    }
}