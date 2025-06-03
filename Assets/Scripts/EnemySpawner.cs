using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 10;

    [Header("Параметры размещения")]
    [SerializeField] private float minSpacing = 2f;
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField] private int maxAttempts = 50;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (spawnArea == null || enemyPrefab == null) return;

        List<Vector3> spawnedPositions = new List<Vector3>();
        Bounds bounds = spawnArea.bounds;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = GetRandomPosition(bounds);
            int attempts = 0;

            while (attempts < maxAttempts &&
                  !IsPositionValid(spawnPos, spawnedPositions, minSpacing))
            {
                spawnPos = GetRandomPosition(bounds);
                attempts++;
            }

            if (attempts < maxAttempts)
            {
                spawnPos.y = bounds.min.y + spawnHeight;
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedPositions.Add(spawnPos);
            }
            else
            {
                Debug.LogWarning($"Не удалось разместить врага {i} после {maxAttempts} попыток");
            }
        }
    }

    Vector3 GetRandomPosition(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    bool IsPositionValid(Vector3 pos, List<Vector3> existingPositions, float minDistance)
    {
        foreach (Vector3 existingPos in existingPositions)
        {
            if (Vector3.Distance(pos, existingPos) < minDistance)
                return false;
        }
        return true;
    }

    void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}