# Match Sweets

A Match 3 game template.

<details open><summary><b>Gameplay</b></summary>
<br />

<!-- https://user-images.githubusercontent.com/28132516/159513210-e6e48b73-1b77-44de-a07d-59cbd8bee1e6.mp4 -->

https://user-images.githubusercontent.com/28132516/160339166-0efb4537-50db-469c-adb1-3bdcd0ee3d8a.mp4

</details>

## :open_book: Table of Contents

- [About](#pencil-about)
- [Folder Structure](#cactus-folder-structure)
- [How To Use](#rocket-how-to-use)
  - [Add new icons set](#add-new-icons-set)
  - [Create animation job](#create-animation-job)
  - [Create fill strategy](#create-fill-strategy)
  - [Create level goal](#create-level-goal)
  - [Create sequence detector](#create-sequence-detector)
- [ToDo](#dart-todo)
- [Contributing](#bookmark_tabs-contributing)
  - [Report a bug](#report-a-bug)
  - [Request a feature](#request-a-feature)
  - [Show your support](#show-your-support)
- [License](#balance_scale-license)

## :pencil: About

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

## :cactus: Folder Structure

    .
    ├── Art
    │   ├── Icons
    │   │   ├── Food
    │   │   └── Sweets
    │   ├── Sprites
    │   └── Textures
    │
    ├── Plugins
    │   └── Match3
    │       ├── App
    │       └── Core
    │
    ├── Prefabs
    │   ├── TilePrefab.prefab
    │   └── ItemPrefab.prefab
    │
    ├── Scenes
    │   └── MainScene.unity
    │
    ├── Scripts
    │   ├── Common
    │   ├── ItemsDrop
    │   ├── ItemsScroll
    │   ├── ItemsRollDown
    │   ├── App.cs
    │   ├── AppContext.cs
    │   └── Game.cs

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
        _items = items;
    }

    public override async UniTask ExecuteAsync()
    {
        var itemsSequence = DOTween.Sequence();

        foreach (var item in _items)
        {
            var destinationPosition = item.GetWorldPosition() + Vector3.right;

            _ = itemsSequence
                .Join(item.Transform.DOMove(destinationPosition, SlideDuration))
                .Join(item.SpriteRenderer.DOFade(0, FadeDuration));
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
        _items = items;
    }

    public override async UniTask ExecuteAsync()
    {
        var itemsSequence = DOTween.Sequence();

        foreach (var item in _items)
        {
            var destinationPosition = item.GetWorldPosition();

            item.SetWorldPosition(destinationPosition + Vector3.left);
            item.Transform.localScale = Vector3.one;
            item.SpriteRenderer.SetAlpha(0);
            item.Show();

            _ = itemsSequence
                .Join(item.Transform.DOMove(destinationPosition, SlideDuration))
                .Join(item.SpriteRenderer.DOFade(1, FadeDuration));
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

First of all, create a class `ItemsSlideFillStrategy` and inherit from the `IBoardFillStrategy<TItem>`.

```csharp
public class ItemsSlideFillStrategy : IBoardFillStrategy<IUnityItem>
{
    private readonly IGameBoardRenderer _gameBoardRenderer;
    private readonly IItemsPool<IUnityItem> _itemsPool;

    public ItemsSlideFillStrategy(IGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool)
    {
        _itemsPool = itemsPool;
        _gameBoardRenderer = gameBoardRenderer;
    }

    public string Name => "Slide Fill Strategy";

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

            var item = _itemsPool.GetItem();
            item.SetWorldPosition(_gameBoardRenderer.GetWorldPosition(rowIndex, columnIndex));

            gridSlot.SetItem(item);
            itemsToShow.Add(item);
        }
    }

    return new[] { new ItemsShowJob(itemsToShow) };
}
```

Next, we implement the `GetSolveJobs` method. This method is used to deal with solved sequences of items.

```csharp
public IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
    IEnumerable<ItemSequence<IUnityItem>> sequences)
{
    var itemsToHide = new List<IUnityItem>();
    var itemsToShow = new List<IUnityItem>();
    var solvedGridSlots = new HashSet<GridSlot<IUnityItem>>();

    foreach (var sequence in sequences)
    {
        foreach (var solvedGridSlot in sequence.SolvedGridSlots)
        {
            if (solvedGridSlots.Add(solvedGridSlot) == false)
            {
                continue;
            }

            var oldItem = solvedGridSlot.Item;
            _itemsPool.ReturnItem(oldItem);

            var newItem = _itemsPool.GetItem();
            newItem.SetWorldPosition(oldItem.GetWorldPosition());
            solvedGridSlot.SetItem(newItem);

            itemsToHide.Add(oldItem);
            itemsToShow.Add(newItem);
        }
    }

    solvedGridSlots.Clear();

    return new IJob[] { new ItemsSlideOutJob(itemsToHide), new ItemsSlideInJob(itemsToShow) };
}
```

Once the `ItemsSlideFillStrategy` is implemented. Register it in the `AppContext` class.

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
            new ItemsSlideFillStrategy(gameBoardRenderer, itemsPool)
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
            new SquareShapeDetector(),
            new VerticalLineDetector(),
            new HorizontalLineDetector()
        });
    }

    ...
}
```

## :dart: ToDo

Here are some features which are either under way or planned:

- [ ] Add tests
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
