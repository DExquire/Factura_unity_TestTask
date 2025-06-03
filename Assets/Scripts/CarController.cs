using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public static CarController Instance { get; private set; }

    public event Action<float> OnDistanceChanged;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform turretMountPoint;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private ParticleSystem fireEffect;

    private Rigidbody _rb;
    public HealthSystem _healthSystem;
    private bool _isMoving = false;
    public float _distanceTraveled = 0;
    public float _maxDistanceTravelled = 10;
    private Vector3 _startPosition;

    public Transform TurretMountPoint => turretMountPoint;
    public bool IsMoving => _isMoving;

    public float DistanceTraveled { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _rb = GetComponent<Rigidbody>();
        _healthSystem = GetComponent<HealthSystem>();
        _startPosition = transform.position;

        _healthSystem.OnHealthChanged.AddListener(UpdateCarEffects);
        _healthSystem.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath() => GameManager.Instance.GameOver();

    private void Start() => GameManager.Instance.InitGame();

    private void Update()
    {
        if (_isMoving)
        {
            _distanceTraveled = Vector3.Distance(_startPosition, transform.position);
            GameManager.Instance.UpdateCarDistance(_distanceTraveled);
            UIManager.Instance.UpdateDistanceUI();


        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
            _rb.velocity = transform.forward * moveSpeed;
        else
            _rb.velocity = Vector3.zero;

    }

    public void StartMoving() => _isMoving = true;
    public void StopMoving() => _isMoving = false;

    private void UpdateCarEffects(float currentHealth, float maxHealth)
    {

        if (smokeEffect == null) return;

        if (currentHealth <= maxHealth * 0.5f)
        {
            if (!smokeEffect.isPlaying) smokeEffect.gameObject.SetActive(true);
        }
        else
        {
            smokeEffect.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            _healthSystem.TakeDamage(10);
    }
}