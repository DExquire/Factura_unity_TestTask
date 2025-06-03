using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnGameFinished = new UnityEvent();

    public enum GameState { MainMenu, Tutorial, Playing, GameOver, Victory }

    [SerializeField] public int levelLength = 1000;
    private GameState _currentState = GameState.MainMenu;
    private float _carDistance = 0;
    private int _coins = 0;

    [System.Serializable]
    public class CoinsEvent : UnityEvent<int> { }
    public CoinsEvent OnCoinsChanged = new CoinsEvent();

    public GameState CurrentState => _currentState;
    public float Progress => Mathf.Clamp01(_carDistance / levelLength);

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 20f;
    [SerializeField] private float spawnAngleRange = 60f;

    public Transform enemyArea;

    public void AddCoins(int amount)
    {
        _coins += amount;
        OnCoinsChanged.Invoke(_coins);
    }

    public void ShowTutorial()
    {
        _currentState = GameState.Tutorial;
    }

    public void StartGame()
    {
        _currentState = GameState.Playing;
        OnGameStart.Invoke();
        _carDistance = 0;
        _coins = 0;
        UIManager.Instance?.SetActiveScreen(UIManager.ScreenType.HUD);
        CameraController.Instance.SwitchToGameplayCamera();
        
        CarController.Instance?.StartMoving();
        UIManager.Instance.healthSlider.gameObject.SetActive(true);
    }
 

    public void InitGame()
    {
        _currentState = GameState.MainMenu;
        _carDistance = 0;
        _coins = 0;
        UIManager.Instance?.SetActiveScreen(UIManager.ScreenType.MainMenu);
        CameraController.Instance?.SwitchToMainMenuCamera();
        UIManager.Instance.healthSlider.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _currentState = GameState.GameOver;
        OnGameFinished?.Invoke();
        CarController.Instance?.StopMoving();
        UIManager.Instance?.SetActiveScreen(UIManager.ScreenType.GameOver);
    }

    public void Victory()
    {
        _currentState = GameState.Victory;
        OnGameFinished?.Invoke();
        CarController.Instance?.StopMoving();
        UIManager.Instance?.SetActiveScreen(UIManager.ScreenType.Victory);
    }

    public void UpdateCarDistance(float distance)
    {
        _carDistance = distance;
        if (_carDistance >= levelLength && _currentState == GameState.Playing)
        {
            Victory();
        }
    }

    public void RestartGame()
    {
        _currentState = GameState.MainMenu;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 0;
    }
}