using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.U2D;
using Random = System.Random;

public class ItemGenerator : MonoBehaviour, IItemGenerator, IDisposable
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private SpriteAtlas _spriteAtlas;

    private Random _random;
    private Sprite[] _sprites;
    private Queue<IItem> _itemsPool;

    public void InitPool(int capacity)
    {
        if (_itemsPool != null)
        {
            Debug.LogError("Items have already been created.");
            return;
        }

        _random = new Random();
        _sprites = GetSprites(_spriteAtlas);
        _itemsPool = new Queue<IItem>(capacity);

        for (var i = 0; i < capacity; i++)
        {
            _itemsPool.Enqueue(CreateItem());
        }
    }

    public IItem GetItem()
    {
        var item = _itemsPool.Dequeue();
        var (index, sprite) = GetRandomSprite();

        item.SetSprite(index, sprite);

        return item;
    }

    public void ReturnItem(IItem item)
    {
        _itemsPool.Enqueue(item);
    }

    public void Dispose()
    {
        foreach (var item in _itemsPool)
        {
            Destroy(item.Transform.gameObject);
        }
        
        _itemsPool.Clear();
    }

    private IItem CreateItem()
    {
        var item = Instantiate(_itemPrefab, transform).GetComponent<IItem>();
        item.Hide();

        return item;
    }

    private (int, Sprite) GetRandomSprite()
    {
        var index = _random.Next(0, _sprites.Length);
        return (index, _sprites[index]);
    }

    private Sprite[] GetSprites(SpriteAtlas spriteAtlas)
    {
        var sprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(sprites);

        return sprites;
    }
}