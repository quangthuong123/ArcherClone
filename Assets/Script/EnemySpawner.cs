using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 3f;
    public float spawnRadius = 10f;

    [Header("Optimization")]
    public int maxEnemies = 6; // Limit to 6 enemies

    // We will count how many enemies are currently alive
    public static int currentEnemyCount = 0;

    IEnumerator Start()
    {
        currentEnemyCount = 0;

        // FIX: Wait 1 second for the MazeGenerator to finish building walls
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn if we are below the limit
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (player == null) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 randomPoint = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            Instantiate(enemyPrefab, hit.position, Quaternion.identity);

            // Increase the count when an enemy is born
            currentEnemyCount++;
        }
    }
}