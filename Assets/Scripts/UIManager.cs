using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public enum ScreenType { MainMenu, Tutorial, GameOver, Victory, HUD }

    [Header("Экраны UI")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject hudScreen;

    [Header("Health Bar")]
    [SerializeField] public Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private Gradient healthGradient;

    [Header("Progress Bar")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text distanceText;
    [SerializeField] private string distanceFormat = "{0}m / {1}m";

    [Header("References")]
    [SerializeField] private CarController carController;
    [SerializeField] private HealthSystem playerHealth;

    private float _maxDistance;
    private float _targetHealthValue;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerHealth.OnHealthChanged.AddListener(UpdateHealthUI);
        _targetHealthValue = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        healthSlider.value = _targetHealthValue;
        healthFill.color = healthGradient.Evaluate(_targetHealthValue);

        _maxDistance = GameManager.Instance.levelLength;
        UpdateDistanceUI();

        ShowMainMenu();
    }

    private void Update()
    {
        if (healthSlider.value != _targetHealthValue)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, _targetHealthValue, 5f * Time.deltaTime);
            healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
        }

        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            UpdateDistanceUI();
        }
    }

    public void GetObjects()
    {
        mainMenuScreen = GameObject.Find("MainMenu");
        victoryScreen = GameObject.Find("GameWin");
        gameOverScreen = GameObject.Find("GameOver");
        tutorialScreen = GameObject.Find("TutorialScreen");
        hudScreen = GameObject.Find("HUD");
    }

    public void SetActiveScreen(ScreenType screenType)
    {
        mainMenuScreen.SetActive(screenType == ScreenType.MainMenu);
        tutorialScreen.SetActive(screenType == ScreenType.Tutorial);
        gameOverScreen.SetActive(screenType == ScreenType.GameOver);
        victoryScreen.SetActive(screenType == ScreenType.Victory);
        hudScreen.SetActive(screenType == ScreenType.HUD);
    }

    public void ShowMainMenu() => SetActiveScreen(ScreenType.MainMenu);
    public void ShowTutorial() => SetActiveScreen(ScreenType.Tutorial);
    public void ShowGameOver() => SetActiveScreen(ScreenType.GameOver);
    public void ShowVictory() => SetActiveScreen(ScreenType.Victory);
    public void ShowHUD() => SetActiveScreen(ScreenType.HUD);

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        _targetHealthValue = currentHealth / maxHealth;
    }

    public void UpdateDistanceUI()
    {
        float distance = carController._distanceTraveled;
        float progress = Mathf.Clamp01(distance / _maxDistance);

        progressSlider.maxValue = GameManager.Instance.levelLength;
        progressSlider.value = distance;
    }

    public void OnStartButtonClicked()
    {
        GameManager.Instance.ShowTutorial();
        ShowTutorial();
    }

    public void OnPlayButtonClicked()
    {
        GameManager.Instance.StartGame();
        ShowHUD();
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnPauseButtonClicked()
    {
        GameManager.Instance.PauseGame();
    }

    public void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
    }
}