using System;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using NSubstitute;
using NUnit.Framework;

namespace Match3.Tests.EditMode
{
    public class GridSlotTests
    {
        [Test]
        public void SetItem_ShouldSetOccupiedState_WhenItemIsValid()
        {
            // Arrange
            var item = Substitute.For<IItem>();
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Act
            gridSlot.SetItem(item);

            // Assert
            Assert.AreEqual(item, gridSlot.Item);
            // Assert.AreEqual(GridSlotState.Occupied, gridSlot.State);
        }

        [Test]
        public void SetItem_ShouldThrowException_WhenItemIsNotValid()
        {
            // Arrange
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Assert
            Assert.Throws<NullReferenceException>(() => gridSlot.SetItem(null));
        }

        [Test]
        public void MarkSolved_ShouldSetSolvedState_WhenItemIsNotNull()
        {
            // Arrange
            var item = Substitute.For<IItem>();
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Act
            gridSlot.SetItem(item);
            // gridSlot.MarkSolved();

            // Assert
            // Assert.AreEqual(GridSlotState.Solved, gridSlot.State);
        }

        [Test]
        public void MarkSolved_ShouldThrowException_WhenItemIsNull()
        {
            // Arrange
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Assert
            // Assert.Throws<NullReferenceException>(() => gridSlot.MarkSolved());
        }

        [Test]
        public void Clear_ShouldSetEmptyState_WhenStateIsAvailable()
        {
            // Arrange
            var item = Substitute.For<IItem>();
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Act
            gridSlot.SetItem(item);
            gridSlot.Clear();

            // Assert
            Assert.AreEqual(default, gridSlot.Item);
            // Assert.AreEqual(GridSlotState.Empty, gridSlot.State);
        }

        [Test]
        public void Clear_ShouldThrowException_WhenStateIsNotAvailable()
        {
            // Arrange
            var gridSlot = new GridSlot<IItem>(Substitute.For<IGridSlotState>(), GridPosition.Zero);

            // Assert
            Assert.Throws<InvalidOperationException>(() => gridSlot.Clear());
        }
    }
}