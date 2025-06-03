using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IPooledObject
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float activationDistance = 15f;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private string attackAnimName = "Hit";
    [SerializeField] private GameObject addCoinTextPrefab;

    private Transform _playerCar;
    private bool _isActive;
    private bool _isAttacking;
    public int _currentHealth;
    private float _attackAnimLength;
    private float _attackEndTime;

    public void OnEnable()
    {
        OnObjectSpawn();
    }

    public void OnObjectSpawn()
    {
        _playerCar = CarController.Instance.transform;
        _isActive = false;
        _currentHealth = maxHealth;

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == attackAnimName)
                {
                    _attackAnimLength = clip.length;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (_playerCar == null)
        {
            _playerCar = CarController.Instance.transform;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _playerCar.position);

        if (!_isActive && distanceToPlayer <= activationDistance && !_isAttacking)
        {
            _isActive = true;
            if (animator != null)
            {
                animator.SetBool("Run", true);
            }
        }

        if (_isActive)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _playerCar.position,
                moveSpeed * Time.deltaTime
            );

            transform.LookAt(_playerCar);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth);
        
        if (_currentHealth <= 0)
        {
            Invoke("Die", _attackAnimLength);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActive)
        {
            _isAttacking = true;
            _isActive = false;
            Attack();
        }
    }

    void Attack()
    {
        _isActive = false;
        CarController.Instance.GetComponent<HealthSystem>().TakeDamage(10);
        
        if (animator != null)
        {
            animator.SetBool("Hit", true);
        }

        Invoke(nameof(ReturnToPool), _attackAnimLength + 1);
    }

    void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public void SpawnText()
    {
        GameObject addCoinText = Instantiate(addCoinTextPrefab, transform);
        addCoinText.transform.position += 2 * Time.deltaTime * Vector3.up;
    }

    private void OnDisable()
    {
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }
}