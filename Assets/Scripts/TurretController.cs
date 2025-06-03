using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxRotationAngle = 60f;

    private float _nextFireTime;
    private bool _isShootingEnabled;
    private Vector3 _mouseStartPos;
    private float _currentRotationY;

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(EnableShooting);
        GameManager.Instance.OnGameFinished.AddListener(DisableShooting);
        _currentRotationY = transform.eulerAngles.y;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart.RemoveListener(EnableShooting);
            GameManager.Instance.OnGameFinished.RemoveListener(DisableShooting);
        }
    }

    private void EnableShooting()
    {
        _isShootingEnabled = true;
    }

    private void DisableShooting()
    {
        _isShootingEnabled = false;
    }

    private void Update()
    {
        if (!_isShootingEnabled) return;

        HandleMouseInput();
        AutoFire();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _mouseStartPos;
            _currentRotationY += delta.x * rotationSpeed * Time.deltaTime;
            _currentRotationY = Mathf.Clamp(_currentRotationY, -maxRotationAngle, maxRotationAngle);

            transform.rotation = Quaternion.Euler(-90, _currentRotationY, 0);
            _mouseStartPos = Input.mousePosition;
        }
    }

    private void AutoFire()
    {
        if (Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        GameObject bullet = ObjectPool.Instance.GetBullet(/*"Bullet",*/ firePoint.position, firePoint.rotation);
        if (bullet != null)
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
        }
    }
}