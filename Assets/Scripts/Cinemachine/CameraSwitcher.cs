using Cinemachine;
using UnityEngine;

public class CameraSwitcher : Singleton<CameraSwitcher>
{
    [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
    [SerializeField] private CinemachineVirtualCamera gameplayCamera;

    public void SwitchToGameplayCamera()
    {
        mainMenuCamera.Priority = 0;
        gameplayCamera.Priority = 10;
    }

    public void SwitchToMainMenuCamera()
    {
        gameplayCamera.Priority = 0;
        mainMenuCamera.Priority = 10;
    }
}