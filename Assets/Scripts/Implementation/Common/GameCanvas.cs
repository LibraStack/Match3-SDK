using System;
using System.Linq;
using Implementation.Common.Interfaces;
using Implementation.Common.UiElements;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common
{
    public class GameCanvas : MonoBehaviour, IGameCanvas
    {
        [SerializeField] private AppContext _appContext;
        [SerializeField] private InteractableDropdown _fillStrategyDropdown;
        [SerializeField] private InteractableButton _startGameButton;

        private IBoardFillStrategy<IUnityItem>[] _boardFillStrategies;

        public event EventHandler StartGameClick;

        private void Awake()
        {
            _boardFillStrategies = _appContext.Resolve<IBoardFillStrategy<IUnityItem>[]>();
        }

        private void Start()
        {
            _fillStrategyDropdown.AddItems(_boardFillStrategies.Select(strategy => strategy.Name));
        }

        private void OnEnable()
        {
            _startGameButton.Click += OnStartGameButtonClick;
        }

        private void OnDisable()
        {
            _startGameButton.Click -= OnStartGameButtonClick;
        }

        public IBoardFillStrategy<IUnityItem> GetSelectedFillStrategy()
        {
            return _boardFillStrategies[_fillStrategyDropdown.SelectedIndex];
        }

        private void OnStartGameButtonClick()
        {
            StartGameClick?.Invoke(this, EventArgs.Empty);
        }
    }
}