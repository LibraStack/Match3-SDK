using System;
using System.Linq;
using Implementation.Common.Interfaces;
using Implementation.Common.LevelGoals;
using Implementation.Common.Models;
using Implementation.Common.UiElements;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common
{
    public class GameUiCanvas : MonoBehaviour, IGameUiCanvas
    {
        [SerializeField] private AppContext _appContext;
        [SerializeField] private InteractableDropdown _iconsSetDropdown;
        [SerializeField] private InteractableDropdown _fillStrategyDropdown;
        [SerializeField] private InteractableButton _startGameButton;

        public int SelectedIconsSetIndex => _iconsSetDropdown.SelectedIndex;
        public int SelectedFillStrategyIndex => _fillStrategyDropdown.SelectedIndex;

        public event EventHandler StartGameClick;

        private void Start()
        {
            _iconsSetDropdown.AddItems(_appContext.Resolve<IconsSetModel[]>().Select(iconsSet => iconsSet.Name));
            _fillStrategyDropdown.AddItems(_appContext.Resolve<IBoardFillStrategy<IUnityItem>[]>()
                .Select(strategy => strategy.Name));
        }

        private void OnEnable()
        {
            _startGameButton.Click += OnStartGameButtonClick;
        }

        private void OnDisable()
        {
            _startGameButton.Click -= OnStartGameButtonClick;
        }

        public void ShowMessage(string message)
        {
            Debug.Log(message);
        }

        public void RegisterAchievedGoal(LevelGoal achievedGoal)
        {
            ShowMessage($"The goal {achievedGoal.GetType().Name} achieved.");
        }

        private void OnStartGameButtonClick()
        {
            StartGameClick?.Invoke(this, EventArgs.Empty);
        }
    }
}