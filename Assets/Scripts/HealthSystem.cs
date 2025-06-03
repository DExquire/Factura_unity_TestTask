using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [System.Serializable]
    public class HealthEvent : UnityEvent<float, float> { }

    [SerializeField] private float maxHealth = 100f;
    public HealthEvent OnHealthChanged = new HealthEvent();
    public UnityEvent OnDeath = new UnityEvent();

    [SerializeField]private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);
        OnHealthChanged.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }
}