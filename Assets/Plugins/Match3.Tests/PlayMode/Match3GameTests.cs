using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.App;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using Match3.Tests.PlayMode.Mocks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Match3.Tests.PlayMode
{
    public class Match3GameTests
    {
        private readonly MockInputSystem _inputSystem;
        private readonly GameConfig<IItem> _gameConfig;

        public Match3GameTests()
        {
            _inputSystem = new MockInputSystem();

            var gameBoardSolver = Substitute.For<IGameBoardSolver<IItem>>();
            gameBoardSolver
                .Solve(Arg.Any<IGameBoard<IItem>>(), Arg.Any<GridPosition[]>())
                .Returns(new ItemSequence<IItem>[1]);

            var gameBoardRenderer = Substitute.For<IGameBoardRenderer>();
            gameBoardRenderer
                .IsPointerOnBoard(Arg.Any<Vector3>(), out _)
                .Returns(args =>
                {
                    var inVector3 = args.Arg<Vector3>();
                    var outGridPosition = new GridPosition((int) inVector3.y, (int) inVector3.x);

                    args[1] = outGridPosition;
                    return true;
                });

            var levelGoalsProvider = Substitute.For<ILevelGoalsProvider<IItem>>();
            levelGoalsProvider
                .GetLevelGoals(Arg.Any<int>(), Arg.Any<IGameBoard<IItem>>())
                .Returns(new LevelGoal<IItem>[] { new MockLevelGoal() });

            var gameBoardDataProvider = Substitute.For<IGameBoardDataProvider>();
            gameBoardDataProvider
                .GetGameBoardData(Arg.Any<int>())
                .Returns(new[,] { { true, true }, { true, true } });

            _gameConfig = new GameConfig<IItem>
            {
                InputSystem = _inputSystem,
                GameBoardSolver = gameBoardSolver,
                GameBoardRenderer = gameBoardRenderer,
                LevelGoalsProvider = levelGoalsProvider,
                GameBoardDataProvider = gameBoardDataProvider,
                ItemSwapper = Substitute.For<IItemSwapper<IItem>>(),
                FillStrategy = Substitute.For<IBoardFillStrategy<IItem>>(),
                SolvedSequencesConsumers = Array.Empty<ISolvedSequencesConsumer<IItem>>()
            };
        }

        [UnityTest]
        public IEnumerator GameCompletionLogic_ShouldRaiseCompletionEvents_WhenAllGoalsAchieved()
        {
            // Arrange
            const string gameFinishedEventName = "Finished";
            const string levelGoalAchievedEventName = "LevelGoalAchieved";

            using var match3Game = new Match3Game<IItem>(_gameConfig);
            match3Game.InitGameLevel(0);

            var receivedEvents = new List<string>();
            match3Game.Finished += (_, __) => receivedEvents.Add(gameFinishedEventName);
            match3Game.LevelGoalAchieved += (_, __) => receivedEvents.Add(levelGoalAchievedEventName);

            var item = Substitute.For<IItem>();
            foreach (var gridSlot in match3Game.GetGridSlots())
            {
                gridSlot.SetItem(item);
            }

            // Act
            yield return match3Game.StartAsync();

            _inputSystem.InvokePointerDown(GridPosition.Zero);
            _inputSystem.InvokePointerDrag(new GridPosition(0, 1));

            yield return null;

            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(levelGoalAchievedEventName, receivedEvents[0]);
            Assert.AreEqual(gameFinishedEventName, receivedEvents[1]);
        }

        [TestCaseSource(nameof(GameBoardDataCases))]
        public void InitGameLevel_ShouldCreateGridSlots_WhenAllParamsAreValid(int level, bool[,] data,
            int expectedResult)
        {
            // Arrange
            var gameBoardDataProvider = Substitute.For<IGameBoardDataProvider>();
            gameBoardDataProvider.GetGameBoardData(level).Returns(data);

            var gameConfig = new GameConfig<IItem>
            {
                GameBoardDataProvider = gameBoardDataProvider,
                LevelGoalsProvider = Substitute.For<ILevelGoalsProvider<IItem>>()
            };

            using var match3Game = new Match3Game<IItem>(gameConfig);

            // Act
            match3Game.InitGameLevel(level);
            var result = match3Game.GetGridSlots().Count();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        private static IEnumerable<object[]> GameBoardDataCases()
        {
            yield return new object[]
            {
                0, new[,] { { true, true }, { true, true } }, 4
            };

            yield return new object[]
            {
                1, new[,] { { true, false, true }, { true, false, true }, { true, false, true } }, 9
            };
        }
    }
}