using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AppContext : MonoBehaviour
{
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private ItemGenerator _itemGenerator;

    private Dictionary<Type, object> _registeredTypes;

    private void Awake()
    {
        _registeredTypes = new Dictionary<Type, object>
        {
            { typeof(IGameBoard), _gameBoard },
            { typeof(IInputSystem), _inputSystem },
            { typeof(IItemGenerator), _itemGenerator },
        };
    }

    public T Resolve<T>()
    {
        return (T)_registeredTypes[typeof(T)];
    }
}