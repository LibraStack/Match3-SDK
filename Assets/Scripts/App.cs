using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;

    private Game _game;

    private void Awake()
    {
        _appContext.Construct();
        _game = new Game(_appContext);
    }

    private void Start()
    {
        _game.Start();
    }

    private void OnEnable()
    {
        _game.Enable();
    }

    private void OnDisable()
    {
        _game.Disable();
    }

    private void OnDestroy()
    {
        _game.Dispose();
    }
}
