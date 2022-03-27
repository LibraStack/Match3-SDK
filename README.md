# Match Sweets

A Match 3 game template.

<details open><summary><b>Gameplay</b></summary>
<br />

https://user-images.githubusercontent.com/28132516/159513210-e6e48b73-1b77-44de-a07d-59cbd8bee1e6.mp4

</details>

## Table of Contents

- [About](#about)
- [How To Use](#how-to-use)
  - [Add New Items Set](#add-new-items-set)
  - [Add New Level Goal](#add-new-level-goal)
- [Show Your Support](#show-your-support)
- [License](#license)

## About

A Match 3 game template with three implementations to fill the playing field.

## How To Use

### Add New Items Set

To add a new items set, simply create a `SpriteAtlas` and add it to the `AppContext` via the Inspector.

![AppContextInspector](https://user-images.githubusercontent.com/28132516/160287440-7c0eba00-c704-4cc1-959c-5044ad924e95.png)

> **Note:** You can change icons size by changing the `Pixels Per Unit` option in the sprite settings.

### Add New Level Goal

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

## Show your support

Give a ⭐ if this project helped you!

<a href="https://www.buymeacoffee.com/chebanovdd" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-orange.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

## License

Usage is provided under the [MIT License](LICENSE).
