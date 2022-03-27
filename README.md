# Match Sweets

A Match 3 game template.

<details open><summary><b>Gameplay</b></summary>
<br />

https://user-images.githubusercontent.com/28132516/159513210-e6e48b73-1b77-44de-a07d-59cbd8bee1e6.mp4

</details>

## Table of Contents

- [About](#about)
  - [Items Scale Fill Strategy](#items-scale-fill-strategy)
  - [Items Drop Fill Strategy](#items-drop-fill-strategy)
  - [Items Roll Down Fill Strategy](#items-roll-down-fill-strategy)
- [How To Use](#how-to-use)
  - [Add New Items Set](#add-new-items-set)
  - [Create New Level Goal](#create-new-level-goal)
  - [Create New Animation Job](#create-new-animation-job)
  - [Change Game Board Solver](#change-game-board-solver)
  - [Create Game Board Fill Strategy](#create-game-board-fill-strategy)
- [Show Your Support](#show-your-support)
- [License](#license)

## About

A Match 3 game template with three implementations to fill the playing field.

### Items Scale Fill Strategy

This strategy generates elements in place of disappeared elements with a scale animation.

<details><summary>Video Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/160290077-d9828baf-aa54-4a26-ad32-116a1599f52a.mp4

</details>

### Items Drop Fill Strategy

This strategy generates elements at the top of each column with a drop animation.

<details><summary>Video Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/160290086-7144056b-a0e8-405c-b393-204990b3d3bd.mp4

</details>

### Items Roll Down Fill Strategy

This strategy generates elements at the top of columns with a rolling down animation.

<details><summary>Video Demonstration</summary>
<br />

https://user-images.githubusercontent.com/28132516/160290097-4fc9ad62-3e17-41d8-860c-2868afd807cd.mp4

</details>

> **Note:** The `ItemsDropFillStrategy` & `ItemsRollDownFillStrategy` are given as an example. Consider to implement an object pooling technique for the `ItemMoveData` to reduce memory pressure.

## How To Use

### Add New Items Set

To add a new items set, simply create a `SpriteAtlas` and add it to the `AppContext` via the Inspector.

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

## Change Game Board Solver

...

## Create Game Board Fill Strategy

...

## Show your support

Give a ⭐ if this project helped you!

<a href="https://www.buymeacoffee.com/chebanovdd" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-orange.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

## License

Usage is provided under the [MIT License](LICENSE).
