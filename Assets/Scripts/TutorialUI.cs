using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialSteps;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button startButton;

    private int _currentStep = 0;

    private void Start()
    {
        UpdateButtons();
        ShowCurrentStep();
    }

    public void NextStep()
    {
        _currentStep++;
        ShowCurrentStep();
        UpdateButtons();
    }

    public void PreviousStep()
    {
        _currentStep--;
        ShowCurrentStep();
        UpdateButtons();
    }

    private void ShowCurrentStep()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].SetActive(i == _currentStep);
        }
    }

    private void UpdateButtons()
    {
        nextButton.interactable = _currentStep < tutorialSteps.Length - 1;
        previousButton.interactable = _currentStep > 0;
        startButton.gameObject.SetActive(_currentStep == tutorialSteps.Length - 1);
    }
}