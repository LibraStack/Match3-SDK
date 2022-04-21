# Match 3 SDK

A cross-platform library that makes it easy to create your own Match 3 game.

![TerminalAndUnityImplementationMac](https://user-images.githubusercontent.com/28132516/164034219-561688ef-c5ed-41f8-b30e-8e66c4eb2dfb.png)

## :open_book: Table of Contents

- [About](#pencil-about)
- [Folder Structure](#cactus-folder-structure)
- [How To Use](#rocket-how-to-use)
  - [Add new icons set](#add-new-icons-set)
  - [Create animation job](#create-animation-job)
  - [Create fill strategy](#create-fill-strategy)
  - [Create level goal](#create-level-goal)
  - [Create sequence detector](#create-sequence-detector)
  - [Create special item](#create-special-item)
- [ToDo](#dart-todo)
- [Contributing](#bookmark_tabs-contributing)
  - [Report a bug](#report-a-bug)
  - [Request a feature](#request-a-feature)
  - [Show your support](#show-your-support)
- [License](#balance_scale-license)

## :pencil: About

The **Match 3 SDK** is designed to speed up the development of Match 3 games. Use the samples as a starting point for creating your own Match 3 game.

### Unity Sample

A Match 3 game sample with three implementations to fill the playing field.

<table>
  <tr>
    <td align="center">Simple Fill Strategy</td>
    <td align="center">Fall Down Fill Strategy</td>
    <td align="center">Slide Down Fill Strategy</td>
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

> **Note:** The `FallDownFillStrategy` & `SlideDownFillStrategy` are given as an example. Consider to implement an object pooling technique for the `ItemMoveData` to reduce memory pressure.

<details><summary>Gameplay Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/164045071-e2038177-1bc2-475c-8dbc-4b4f77d6895b.mp4

</details>



### Terminal Sample

A Match 3 game sample designed for text terminals.

<details><summary>Gameplay Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/164049550-467590dc-bbf8-4109-a1bb-38dfe6674cd6.mp4

</details>

> **Note:** The sample was tested using Rider's internal console. If you have a problem displaying symbols, configure your terminal to support [Unicode](https://en.wikipedia.org/wiki/Unicode) (in [UTF-8](https://en.wikipedia.org/wiki/UTF-8) form).

## :cactus: Folder Structure

    .
    ├── samples
    │   ├── Terminal.Match3
    │   └── Unity.Match3
    │
    ├── src
    │   ├── Match3.App
    │   ├── Match3.Core
    │   ├── Match3.Template
    │   └── Match3.UnityPackage
    │
    ├── Match3.sln

## :rocket: How To Use

### Add new icons set

To add a new icons set, simply create a `SpriteAtlas` and add it to the `AppContext` via the Inspector.

![AppContextInspector](https://user-images.githubusercontent.com/28132516/160287440-7c0eba00-c704-4cc1-959c-5044ad924e95.png)

> **Note:** You can change icons size by changing the `Pixels Per Unit` option in the sprite settings.

### Create animation job

Let's create a `SlideIn` animation to show the items and a `SlideOut` animation to hide the items. These animations will be used further.

Сreate a class `ItemsSlideOutJob` and inherit from the `Job`.

```csharp
public class ItemsSlideOutJob : Job
{
    private const float FadeDuration = 0.15f;
    private const float SlideDuration = 0.2f;

    private readonly IEnumerable<IUnityItem> _items;

    public ItemsSlideOutJob(IEnumerable<IUnityItem> items, int executionOrder = 0) : base(executionOrder)
    {
        _items = items; // Items to animate.
    }

    public override async UniTask ExecuteAsync()
    {
        var itemsSequence = DOTween.Sequence();

        foreach (var item in _items)
        {
            // Calculate the item destination position.
            var destinationPosition = item.GetWorldPosition() + Vector3.right;

            _ = itemsSequence
                .Join(item.Transform.DOMove(destinationPosition, SlideDuration)) // Smoothly move the item.
                .Join(item.SpriteRenderer.DOFade(0, FadeDuration)); // Smoothly hide the item.
        }

        await itemsSequence.SetEase(Ease.Flash);
    }
}
```

Then create a class `ItemsSlideInJob`.

```csharp
public class ItemsSlideInJob : Job
{
    private const float FadeDuration = 0.15f;
    private const float SlideDuration = 0.2f;

    private readonly IEnumerable<IUnityItem> _items;

    public ItemsSlideInJob(IEnumerable<IUnityItem> items, int executionOrder = 0) : base(executionOrder)
    {
        _items = items; // Items to animate.
    }

    public override async UniTask ExecuteAsync()
    {
        var itemsSequence = DOTween.Sequence();

        foreach (var item in _items)
        {
            // Save the item current position.
            var destinationPosition = item.GetWorldPosition();

            // Move the item to the starting position.
            item.SetWorldPosition(destinationPosition + Vector3.left);
                      
            // Reset the sprite alpha to zero.
            item.SpriteRenderer.SetAlpha(0);
            
            // Reset the item scale.
            item.SetScale(1);
            
            // Activate the item game object.
            item.Show();

            _ = itemsSequence
                .Join(item.Transform.DOMove(destinationPosition, SlideDuration)) // Smoothly move the item.
                .Join(item.SpriteRenderer.DOFade(1, FadeDuration)); // Smoothly show the item.
        }

        await itemsSequence.SetEase(Ease.Flash);
    }
}
```

Jobs with the same `executionOrder` run in parallel. Otherwise, they run one after the other according to the `executionOrder`.

<details><summary>Execution Order Demonstration</summary>
<br />
  
  <table>
    <tr>
      <td align="center">SlideOutJob: 0 <br /> SlideInJob: 0</td>
      <td align="center">SlideOutJob: 0 <br /> SlideInJob: 1</td>
    </tr>
    <tr>
      <td>
        <img src="https://user-images.githubusercontent.com/28132516/160768394-60a238ca-dd67-413e-bd16-941e80ba295c.gif" alt="ItemsSlideAnimation" />
      </td>
      <td>
        <img src="https://user-images.githubusercontent.com/28132516/160781807-7ec6ee2e-7474-4d14-a516-21fae758fa50.gif" alt="ItemsSlideAnimation" />
      </td>
    </tr>
  </table>
</details>
  
### Create fill strategy

First of all, create a class `SidewayFillStrategy` and inherit from the `IBoardFillStrategy<TItem>`.

We'll need an `IGameBoardRenderer` to transform grid positions to world positions and an `IItemsPool<TItem>` to get the pre-created items from the pool. Let's pass them to the constructor.

```csharp
public class SidewayFillStrategy : IBoardFillStrategy<IUnityItem>
{
    private readonly IGameBoardRenderer _gameBoardRenderer;
    private readonly IItemsPool<IUnityItem> _itemsPool;

    public SidewayFillStrategy(IGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool)
    {
        _itemsPool = itemsPool;
        _gameBoardRenderer = gameBoardRenderer;
    }

    public string Name => "Sideway Fill Strategy";

    public IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
        IEnumerable<ItemSequence<IUnityItem>> sequences)
    {
        throw new NotImplementedException();
    }
}
```

Then let's implement the `GetFillJobs` method. This method is used to fill the playing field.

```csharp
public IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
{
    // List of items to show.
    var itemsToShow = new List<IUnityItem>();

    for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
    {
        for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
        {
            var gridSlot = gameBoard[rowIndex, columnIndex];
            if (gridSlot.State != GridSlotState.Empty)
            {
                continue;
            }

            // Get an item from the pool.
            var item = _itemsPool.GetItem();
            
            // Set the position of the item.
            item.SetWorldPosition(_gameBoardRenderer.GetWorldPosition(rowIndex, columnIndex));

            // Set the item to the grid slot.
            gridSlot.SetItem(item);
            
            // Add the item to the list to show.
            itemsToShow.Add(item);
        }
    }

    // Create a job to show items.
    return new[] { new ItemsShowJob(itemsToShow) };
}
```

Next, we implement the `GetSolveJobs` method. This method is used to deal with solved sequences of items.

```csharp
public IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
    IEnumerable<ItemSequence<IUnityItem>> sequences)
{
    // List of items to hide.
    var itemsToHide = new List<IUnityItem>();
    
    // List of items to show.
    var itemsToShow = new List<IUnityItem>();

    foreach (var solvedGridSlot in sequences.GetUniqueGridSlots())
    {
        // Get a new item from the pool.
        var newItem = _itemsPool.GetItem();
        
        // Get the current item of the grid slot.
        var currentItem = solvedGridSlot.Item;

        // Set the position of the new item.
        newItem.SetWorldPosition(currentItem.GetWorldPosition());
        
        // Set the new item to the grid slot.
        solvedGridSlot.SetItem(newItem);

        // Add the current item to the list to hide.
        itemsToHide.Add(currentItem);
        
        // Add the new item to the list to show.
        itemsToShow.Add(newItem);
        
        // Return the current item to the pool.
        _itemsPool.ReturnItem(currentItem);
    }

    // Create jobs to hide and show items using the animations we created above.
    return new IJob[] { new ItemsSlideOutJob(itemsToHide), new ItemsSlideInJob(itemsToShow) };
}
```

Once the `SidewayFillStrategy` is implemented. Register it in the `AppContext` class.

```csharp
public class AppContext : MonoBehaviour, IAppContext
{
    ...

    private IBoardFillStrategy<IUnityItem>[] GetBoardFillStrategies(IGameBoardRenderer gameBoardRenderer,
        IItemsPool<IUnityItem> itemsPool)
    {
        return new IBoardFillStrategy<IUnityItem>[]
        {
            ...
            new SidewayFillStrategy(gameBoardRenderer, itemsPool)
        };
    }
    
    ...
}
```

<details><summary>Video Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/160768194-4dd0688d-b91c-4130-94fc-a49e951898db.mp4

</details>

### Create level goal

Let's say we want to add a goal to collect a certain number of specific items. First of all, create a class `CollectItems` and inherit from the `LevelGoal<TItem>`.

```csharp
public class CollectItems : LevelGoal<IUnityItem>
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
        foreach (var solvedGridSlot in sequences.GetUniqueGridSlots())
        {
            if (solvedGridSlot.Item.ContentId == _contentId)
            {
                _collectedItemsCount++;
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
public class LevelGoalsProvider : ILevelGoalsProvider<IUnityItem>
{
    public LevelGoal<IUnityItem>[] GetLevelGoals(int level, IGameBoard<IUnityItem> gameBoard)
    {
        return new LevelGoal<IUnityItem>[]
        {
            ...
            new CollectItems(0, 25)
        };
    }
}
```

### Create sequence detector

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
        foreach (var lookupDirection in lookupDirections)
        {
            var position = gridPosition + lookupDirection;
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
            ...
            new SquareShapeDetector()
        });
    }

    ...
}
```

### Create special item

Let's create a stone item that is only destroyed when a match happens in one of the neighbour tiles.

Add a `Stone` value to the `TileGroup` enum.

```csharp
public enum TileGroup
{
    Unavailable = 0,
    Available = 1,
    Ice = 2,
    Stone = 3
}
```

Create a class `StoneState` and inherit from the `StatefulGridTile`.

```csharp
public class StoneState : StatefulGridTile
{
    private bool _isLocked = true;
    private bool _canContainItem;
    private TileGroup _group = TileGroup.Stone;

    // Prevents the block from move.
    public override bool IsLocked => _isLocked;
    
    // Prevents the item creation.
    public override bool CanContainItem => _canContainItem;
    
    // Defines the tile group.
    public override TileGroup Group => _group;

    // Occurs when all block states have completed.
    protected override void OnComplete()
    {
        _isLocked = false;
        _canContainItem = true;
        _group = TileGroup.Available;
    }

    // Occurs when the block state is reset.
    protected override void OnReset()
    {
        _isLocked = true;
        _canContainItem = false;
        _group = TileGroup.Stone;
    }
}
```

To respond to any changes in one of the neighbour tiles, we have to implement an `ISpecialItemDetector<TGridSlot>` interface. Create a `StoneItemDetector` class and inherit from the `ISpecialItemDetector<TGridSlot>`.

```csharp
public class StoneItemDetector : ISpecialItemDetector<IUnityGridSlot>
{
    private readonly GridPosition[] _lookupDirections;
    private readonly IUnityGameBoardRenderer _gameBoardRenderer;

    public StoneItemDetector(IUnityGameBoardRenderer gameBoardRenderer)
    {
        _gameBoardRenderer = gameBoardRenderer;
        _lookupDirections = new[]
        {
            GridPosition.Up,
            GridPosition.Down,
            GridPosition.Left,
            GridPosition.Right
        };
    }

    public IEnumerable<IUnityGridSlot> GetSpecialItemGridSlots(IGameBoard<IUnityGridSlot> gameBoard,
        IUnityGridSlot gridSlot)
    {
        if (gridSlot.IsMovable == false)
        {
            yield break;
        }

        foreach (var lookupDirection in _lookupDirections)
        {
            var position = gridSlot.GridPosition + lookupDirection;

            if (!_gameBoardRenderer.IsPositionOnGrid(position) ||
                _gameBoardRenderer.GetTileGroup(position) != TileGroup.Stone)
            {
                continue;
            }

            var hasNextState = _gameBoardRenderer.TrySetNextTileState(position);
            if (hasNextState)
            {
                continue;
            }

            yield return gameBoard[position];
        }
    }
}
```

Once the `StoneItemDetector` is implemented. Register it in the `AppContext` class.

```csharp
public class AppContext : MonoBehaviour, IAppContext
{
    ...

    private ISpecialItemDetector<IUnityGridSlot>[] GetSpecialItemDetectors(IUnityGameBoardRenderer gameBoardRenderer)
    {
        return new ISpecialItemDetector<IUnityGridSlot>[]
        {
            ...
            new StoneItemDetector(gameBoardRenderer)
        };
    }
    
    ...
}
```

Next, move on to setting up the scene and prefabs.

First of all, add a block state sprites to the `TilesSpriteAtlas` and create a `StoneTilePrefab` prefab varian from the `StatefulBlankPrefab`.

<details><summary>Prefab Variant Creation</summary>
<br />

![CreatePrefabVariant](https://user-images.githubusercontent.com/28132516/164171867-3f8b90bf-98d0-482f-bd9e-8d5209932398.png)

</details>

Configure the `StoneTilePrefab` by adding the `StoneState` script to it and filling in a `State Sprite Names` list.

![ConfigureStoneTilePrefab](https://user-images.githubusercontent.com/28132516/164176865-91287ec1-abf5-4c9b-989f-4ee52b2c2630.png)

> **Note:** You can create more than one visual state for a block by adding more state sprites.

Finally, select a `GameBoard` object in the scene and add the `StoneTilePrefab` to a `GridTiles` list of the `UnityGameBoardRenderer` script.

<details><summary>Video Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/164196506-80ebe446-7a7a-4ae6-930c-46586f1b2c25.mp4

</details>

## :dart: ToDo

Here are some features which are either under way or planned:

- [ ] Add tests
- [x] Add special item support
- [ ] Build .unitypackage
- [ ] Publish on Asset Store
- [ ] Optimize `ItemsDrop` & `ItemsRollDown` fill strategies

## :bookmark_tabs: Contributing

You may contribute in several ways like creating new features, fixing bugs or improving documentation and examples. Find more information in *CONTRIBUTING.md*.

### Report a bug

If you find a bug in the source code, please [create bug report](https://github.com/ChebanovDD/MatchSweets/issues/new?assignees=ChebanovDD&labels=bug&template=bug_report.md&title=).

> Please browse [existing issues](https://github.com/ChebanovDD/MatchSweets/issues) to see whether a bug has previously been reported.

### Request a feature

If you have an idea, or you're missing a capability that would make development easier, please [submit feature request](https://github.com/ChebanovDD/MatchSweets/issues/new?assignees=ChebanovDD&labels=enhancement&template=feature_request.md&title=).

> If a similar feature request already exists, don't forget to leave a "+1" or add additional information, such as your thoughts and vision about the feature.

### Show your support

Give a :star: if this project helped you!

<a href="https://www.buymeacoffee.com/chebanovdd" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-orange.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

## :balance_scale: License

Usage is provided under the [MIT License](LICENSE).
