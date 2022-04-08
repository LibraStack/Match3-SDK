using System;
using Match3.App.Internal;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using NUnit.Framework;

namespace Match3.Tests.EditMode
{
    public class GameBoardTests
    {
        private readonly bool[,] _gameBoardData;

        public GameBoardTests()
        {
            _gameBoardData = new[,]
            {
                { true, true, false, false, false },
                { true, true, false, false, false },
                { true, true, false, false, false },
                { true, true, false, false, false },
                { true, true, false, false, false }
            };
        }

        [Test]
        public void CreateGridSlots_ShouldCreateGridSlots_WhenAllParamsAreValid()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var positionOnBoard = new GridPosition(0, 0);
            var positionOutOfBoard = new GridPosition(0, 2);

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);

            // Assert
            Assert.AreEqual(5, gameBoard.RowCount);
            Assert.AreEqual(5, gameBoard.ColumnCount);

            // Assert.AreEqual(GridSlotState.Empty, gameBoard[positionOnBoard].State);
            // Assert.AreEqual(GridSlotState.NotAvailable, gameBoard[positionOutOfBoard].State);
            // Assert.AreEqual(gameBoard[positionOnBoard.RowIndex, positionOnBoard.ColumnIndex].State,
                // gameBoard[positionOnBoard].State);
        }

        [Test]
        public void CreateGridSlots_ShouldThrowException_WhenCallTwice()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);

            // Assert
            // Assert.Throws<InvalidOperationException>(() => gameBoard.CreateGridSlots(_gameBoardData));
        }

        [Test]
        public void IsPositionOnGrid_ShouldReturnTrue_WhenPositionOnGrid()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = GridPosition.Zero;

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);
            var result = gameBoard.IsPositionOnGrid(gridPosition);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsPositionOnGrid_ShouldReturnFalse_WhenPositionOutOfGrid()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = new GridPosition(5, 5);

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);
            var result = gameBoard.IsPositionOnGrid(gridPosition);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsPositionOnGrid_ShouldThrowException_WhenGridSlotsAreNotCreated()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = GridPosition.Zero;

            // Assert
            Assert.Throws<InvalidOperationException>(() => gameBoard.IsPositionOnGrid(gridPosition));
        }

        [Test]
        public void IsPositionOnBoard_ShouldReturnTrue_WhenPositionOnBoard()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = GridPosition.Zero;

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);

            var isPositionOnGrid = gameBoard.IsPositionOnGrid(gridPosition);
            var isPositionOnBoard = gameBoard.IsPositionOnBoard(gridPosition);

            // Assert
            Assert.IsTrue(isPositionOnGrid);
            Assert.IsTrue(isPositionOnBoard);
        }

        [Test]
        public void IsPositionOnBoard_ShouldReturnFalse_WhenPositionOutOfBoard()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = new GridPosition(0, 2);

            // Act
            // gameBoard.CreateGridSlots(_gameBoardData);

            var isPositionOnGrid = gameBoard.IsPositionOnGrid(gridPosition);
            var isPositionOnBoard = gameBoard.IsPositionOnBoard(gridPosition);

            // Assert
            Assert.IsTrue(isPositionOnGrid);
            Assert.IsFalse(isPositionOnBoard);
        }

        [Test]
        public void IsPositionOnBoard_ShouldThrowException_WhenGridSlotsAreNotCreated()
        {
            // Arrange
            using var gameBoard = CreateGameBoard();
            var gridPosition = GridPosition.Zero;

            // Assert
            Assert.Throws<InvalidOperationException>(() => gameBoard.IsPositionOnBoard(gridPosition));
        }

        private GameBoard<IItem> CreateGameBoard()
        {
            return new GameBoard<IItem>(null, null);
        }
    }
}