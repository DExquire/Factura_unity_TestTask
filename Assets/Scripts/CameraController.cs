using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [Header("Camera Positions")]
    [SerializeField] private Transform mainMenuCameraPos;
    [SerializeField] private Transform gameplayCameraPos;
    [SerializeField] private float transitionSpeed = 5f;

    [Header("Gameplay Camera Settings")]
    [SerializeField] private Vector3 gameplayCameraOffset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float followSmoothness = 5f;
    [SerializeField] private float rotationSmoothness = 3f;

    private Transform _target;
    [SerializeField] private Transform _lookAtTarget;
    private bool _isTransitioning = false;
    private bool _isGameplayCamera = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchToMainMenuCamera();
        transform.SetPositionAndRotation(_target.position, _target.rotation);
    }

    private void LateUpdate()
    {
        if (_isTransitioning)
        {
            HandleCameraTransition();
        }
        else if (_isGameplayCamera && _lookAtTarget != null)
        {
            FollowPlayer();
        }
    }

    private void HandleCameraTransition()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, Time.deltaTime * transitionSpeed);

        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            _isTransitioning = false;
        }
    }

    private void FollowPlayer()
    {

        Vector3 targetPosition = _lookAtTarget.position + gameplayCameraOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSmoothness * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(_lookAtTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    public void SwitchToGameplayCamera()
    {
        transform.LookAt(_lookAtTarget);
        _target = gameplayCameraPos;
        _isGameplayCamera = true;
        _isTransitioning = true;
    }

    public void SwitchToMainMenuCamera()
    {
        transform.LookAt(_lookAtTarget);
        _target = mainMenuCameraPos;
        _isGameplayCamera = false;
        _isTransitioning = true;
    }
}