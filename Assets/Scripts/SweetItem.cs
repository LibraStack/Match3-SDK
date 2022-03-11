using Interfaces;
using UnityEngine;

public class SweetItem : MonoBehaviour, IItem
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Vector3 _position;
    
    public int SpriteIndex { get; private set; }
    public Transform Transform => transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSprite(int index, Sprite sprite)
    {
        SpriteIndex = index;
        _spriteRenderer.sprite = sprite;
    }

    public void SetWorldPosition(Vector3 position)
    {
        _position = position;
        transform.position = position;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;// _position; // TODO: Fix bug.
    }
}
