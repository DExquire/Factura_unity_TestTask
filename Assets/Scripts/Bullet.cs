using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    [Header("Настройки пули")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private int damage = 10;

    private Vector3 _startPosition;
    private Vector3 _direction;

    public void OnObjectSpawn()
    {
        _startPosition = transform.position;
        _direction = transform.forward;

        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;

        if (Vector3.Distance(_startPosition, transform.position) >= maxDistance)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log(enemy._currentHealth / 2);
                enemy.TakeDamage(10);

                if(enemy._currentHealth <= 0)
                {
                    CoinManager.Instance.AddCoins(10);
                    enemy.SpawnText();
                }
            }
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
    }
}