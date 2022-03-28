# Match Sweets

A Match 3 game template.

<details open><summary><b>Gameplay</b></summary>
<br />

<!-- https://user-images.githubusercontent.com/28132516/159513210-e6e48b73-1b77-44de-a07d-59cbd8bee1e6.mp4 -->

https://user-images.githubusercontent.com/28132516/160339166-0efb4537-50db-469c-adb1-3bdcd0ee3d8a.mp4

</details>

## Table of Contents

- [About](#about)
- [How To Use](#how-to-use)
  - [Add New Icons Set](#add-new-icons-set)
  - [Create New Level Goal](#create-new-level-goal)
  - [Create New Animation Job](#create-new-animation-job)
  - [Create New Sequence Detector](#create-new-sequence-detector)
  - [Create Game Board Fill Strategy](#create-game-board-fill-strategy)
- [Show Your Support](#show-your-support)
- [License](#license)

## About

A Match 3 game template with three implementations to fill the playing field. Use this project as a starting point for creating your own Match 3 game.

<table>
  <tr>
    <td align="center">Items Scale Fill Strategy</td>
    <td align="center">Items Drop Fill Strategy</td>
    <td align="center">Items Roll Down Fill Strategy</td>
  </tr>
  <tr>
    <td>
      <img src="https://user-images.githubusercontent.com/28132516/160335127-a0a5d43a-cd68-41a9-aa5f-b4d9ec4c5b68.gif" alt="ItemsScaleStrategy" />
    </td>
    <td>
      <img src="https://user-images.githubusercontent.com/28132516/160335148-12c803ad-57e3-45bc-905d-3e537ec2e838.gif" alt="ItemsDropStrategy" />
    </td>
    <td>
      <img src="https://user-images.githubusercontent.com/28132516/160335158-ffb94577-74ff-4d81-8bb5-c995cbc6257d.gif" alt="ItemsRollDownStrategy" />
    </td>
  </tr>
</table>

> **Note:** The `ItemsDropFillStrategy` & `ItemsRollDownFillStrategy` are given as an example. Consider to implement an object pooling technique for the `ItemMoveData` to reduce memory pressure.

## How To Use

### Add New Icons Set

To add a new icons set, simply create a `SpriteAtlas` and add it to the `AppContext` via the Inspector.

![AppContextInspector](https://user-images.githubusercontent.com/28132516/160287440-7c0eba00-c704-4cc1-959c-5044ad924e95.png)

> **Note:** You can change icons size by changing the `Pixels Per Unit` option in the sprite settings.

### Create New Level Goal

Let's say we want to add a goal to collect a certain number of specific items. First of all, create a class `CollectItems` and inherit from the `LevelGoal`.

```csharp
public class CollectItems : LevelGoal
{
    private readonly int _contentId;
    private readonly int _itemsCount;

    private int _collectedItemsCount;

    public CollectItems(int contentId, int itemsCount)
    {
        _contentId = contentId;
        _itemsCount = itemsCount;
    }

    public override void RegisterSolvedSequences(IEnumerable<ItemSequence<IUnityItem>> sequences)
    {
        foreach (var sequence in sequences)
        {
            foreach (var solvedGridSlot in sequence.SolvedGridSlots)
            {
                if (solvedGridSlot.Item.ContentId == _contentId)
                {
                    _collectedItemsCount++;
                }
            }
        }

        if (_collectedItemsCount >= _itemsCount)
        {
            MarkAchieved();
        }
    }
}
```

Once the level goal is implemented. Don't forget to register it in the `LevelGoalsProvider`.

```csharp
public class LevelGoalsProvider : ILevelGoalsProvider
{
    public LevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard)
    {
        return new LevelGoal[]
        {
            new CollectItems(0, 25),
            new CollectRowMaxItems(gameBoard)
        };
    }
}
```
> **Note:** You can modify the `LevelGoalsProvider` to return goals for a certain level, for example.

## Create New Animation Job

...

## Create New Sequence Detector

Let's implement a new sequence detector to detect square shapes. Create a class `SquareShapeDetector` and inherit from the `ISequenceDetector<TItem>`.

First of all, we have to declare an array of lookup directions.

```csharp
public class SquareShapeDetector : ISequenceDetector<IUnityItem>
{
    private readonly GridPosition[][] _squareLookupDirections;

    public SquareShapeDetector()
    {
        _squareLookupDirections = new[]
        {
            new[] { GridPosition.Up, GridPosition.Left, GridPosition.Up + GridPosition.Left },
            new[] { GridPosition.Up, GridPosition.Right, GridPosition.Up + GridPosition.Right },
            new[] { GridPosition.Down, GridPosition.Left, GridPosition.Down + GridPosition.Left },
            new[] { GridPosition.Down, GridPosition.Right, GridPosition.Down + GridPosition.Right },
        };
    }

    public ItemSequence<IUnityItem> GetSequence(IGameBoard<IUnityItem> gameBoard, GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }
}
```

Then let's implement the `GetSequence` method.

```csharp
public ItemSequence<IUnityItem> GetSequence(IGameBoard<IUnityItem> gameBoard, GridPosition gridPosition)
{
    var sampleGridSlot = gameBoard[gridPosition];
    var gridSlots = new List<GridSlot<IUnityItem>>(4);

    foreach (var lookupDirections in _squareLookupDirections)
    {
        foreach (var direction in lookupDirections)
        {
            var position = gridPosition + direction;
            if (gameBoard.IsPositionOnBoard(position) == false)
            {
                break;
            }

            var gridSlot = gameBoard[position];
            if (gridSlot.Item == null)
            {
                break;
            }

            if (gridSlot.Item.ContentId == sampleGridSlot.Item.ContentId)
            {
                gridSlots.Add(gridSlot);
            }
        }

        if (gridSlots.Count == 3)
        {
            gridSlots.Add(sampleGridSlot);
            break;
        }

        gridSlots.Clear();
    }

    return gridSlots.Count > 0 ? new ItemSequence<IUnityItem>(GetType(), gridSlots) : null;
}
```

Finally, add the `SquareShapeDetector` to the sequence detector list of the `GameBoardSolver` constructor in the `AppContext` class.

```csharp
public class AppContext : MonoBehaviour, IAppContext
{
    ...

    private IGameBoardSolver<IUnityItem> GetGameBoardSolver()
    {
        return new GameBoardSolver(new ISequenceDetector<IUnityItem>[]
        {
            new SquareShapeDetector(),
            new VerticalLineDetector(),
            new HorizontalLineDetector()
        });
    }

    ...
}
```

## Create Game Board Fill Strategy

...

## Show your support

Give a ⭐ if this project helped you!

<a href="https://www.buymeacoffee.com/chebanovdd" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-orange.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

## License

Usage is provided under the [MIT License](LICENSE).
