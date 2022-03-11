using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FillStrategies;
using Interfaces;
using UiElements;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;
    [SerializeField] private InteractableDropdown _fillStrategyDropdown;
    [SerializeField] private InteractableButton _fillGameBoardButton;

    private IGameBoard _gameBoard;
    private IInputSystem _inputSystem;
    private IItemGenerator _itemGenerator;
    private IBoardFillStrategy[] _boardFillStrategies;

    private bool _isDragMode;
    private GridPosition _slotDownPosition;
    
    private readonly int[,] _gameBoardData =
    {
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 0, 1, 1, 1}
    };

    private void Awake()
    {
        _gameBoard = _appContext.Resolve<IGameBoard>();
        _inputSystem = _appContext.Resolve<IInputSystem>();
        _itemGenerator = _appContext.Resolve<IItemGenerator>();
        _boardFillStrategies = GetBoardFillStrategies(_gameBoard, _itemGenerator);
        
        DOTween.SetTweensCapacity(200, 100);
    }

    private void Start()
    {
        _gameBoard.Create(_gameBoardData);
        _itemGenerator.InitPool(_gameBoard.RowCount * _gameBoard.ColumnCount + 25);
        _fillStrategyDropdown.AddItems(_boardFillStrategies.Select(strategy => strategy.Name));
    }

    private void OnEnable()
    {
        _inputSystem.PointerDown += OnPointerDown;
        _inputSystem.PointerDrag += OnPointerDrag;
        _fillGameBoardButton.Click += OnFillGameBoardClick;
    }

    private void OnDisable()
    {
        _inputSystem.PointerDown -= OnPointerDown;
        _inputSystem.PointerDrag -= OnPointerDrag;
        _fillGameBoardButton.Click -= OnFillGameBoardClick;
    }

    private void OnPointerDown(object sender, Vector2 mouseWorldPosition)
    {
        if (_gameBoard.IsFilled && 
            _gameBoard.IsPositionOnBoard(mouseWorldPosition, out _slotDownPosition))
        {
            _isDragMode = true;
        }
    }

    private void OnPointerDrag(object sender, Vector2 mouseWorldPosition)
    {
        if (_isDragMode == false)
        {
            return;
        }

        if (_gameBoard.IsPositionOnBoard(mouseWorldPosition, out var slotPosition) == false)
        {
            _isDragMode = false;
            
            return;
        }

        if (IsSameSlot(slotPosition) || IsDiagonalSlot(slotPosition))
        {
            return;
        }
        
        _isDragMode = false;
        _gameBoard.SwapItemsAsync(GetSelectedLoadStrategy(), _slotDownPosition, slotPosition).Forget();
    }

    private void OnFillGameBoardClick()
    {
        _gameBoard.FillAsync(GetSelectedLoadStrategy()).Forget();
    }

    private bool IsSameSlot(GridPosition slotPosition)
    {
        return _slotDownPosition.Equals(slotPosition);
    }

    private bool IsDiagonalSlot(GridPosition slotPosition)
    {
        return slotPosition.Equals(_slotDownPosition - (GridPosition.Up + GridPosition.Left)) ||
               slotPosition.Equals(_slotDownPosition - (GridPosition.Up + GridPosition.Right)) ||
               slotPosition.Equals(_slotDownPosition - (GridPosition.Down + GridPosition.Left)) ||
               slotPosition.Equals(_slotDownPosition - (GridPosition.Down + GridPosition.Right));
    }
    
    private IBoardFillStrategy[] GetBoardFillStrategies(IGrid gameBoard, IItemGenerator itemGenerator)
    {
        return new IBoardFillStrategy[]
        {
            new ItemsScaleFillStrategy(gameBoard, itemGenerator),
            new ItemsDropFillStrategy(gameBoard, itemGenerator)
        };
    }

    private IBoardFillStrategy GetSelectedLoadStrategy()
    {
        return _boardFillStrategies[_fillStrategyDropdown.SelectedIndex];
    }
}