using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Настройки пула пуль")]
    [SerializeField] private Pool bulletPool;
    private Queue<GameObject> _bulletPool;

    private void Start()
    {
        
        _bulletPool = new Queue<GameObject>();
        for (int i = 0; i < bulletPool.size; i++)
        {
            GameObject bullet = Instantiate(bulletPool.prefab);
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if (_bulletPool.Count == 0)
        {
            ExpandBulletPool();
        }

        GameObject bullet = _bulletPool.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.GetComponent<IPooledObject>()?.OnObjectSpawn();

        return bullet;
    }

    private void ExpandBulletPool()
    {
        GameObject bullet = Instantiate(bulletPool.prefab);
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}