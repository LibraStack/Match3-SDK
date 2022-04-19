using System;
using Cysharp.Threading.Tasks;
using Match3.App;
using Match3.Template.Interfaces;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.GameModes
{
    public class GamePlayMode : IGameMode, IDeactivatable
    {
        private readonly TerminalGame _terminalGame;
        private readonly ITerminalInputSystem _inputSystem;

        public GamePlayMode(TerminalGame terminalGame, ITerminalInputSystem inputSystem)
        {
            _terminalGame = terminalGame;
            _inputSystem = inputSystem;
        }

        public event EventHandler Finished;

        public void Activate()
        {
            _terminalGame.StartAsync().Forget();
            _inputSystem.StartMonitoring();

            _inputSystem.Break += OnBreakKeyPressed;
            _terminalGame.Finished += OnGameFinished;
            _terminalGame.LevelGoalAchieved += OnLevelGoalAchieved;
        }

        public void Deactivate()
        {
            _terminalGame.StopAsync().Forget();
            _inputSystem.StopMonitoring();

            _inputSystem.Break -= OnBreakKeyPressed;
            _terminalGame.Finished -= OnGameFinished;
            _terminalGame.LevelGoalAchieved -= OnLevelGoalAchieved;
        }

        private void OnBreakKeyPressed(object sender, EventArgs key)
        {
            RaiseGameFinished();
        }

        private void OnGameFinished(object sender, EventArgs e)
        {
            RaiseGameFinished();
        }

        private void RaiseGameFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void OnLevelGoalAchieved(object sender, LevelGoal<ITerminalGridSlot> levelGoal)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"The goal {levelGoal.GetType().Name} achieved.");
        }
    }
}