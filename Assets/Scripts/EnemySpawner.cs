using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private GameObject spawnCapsulePrefab;

    [SerializeField] private float spawnInterval = 5.0f;

    private float spawnTimer = 5.0f;

    [SerializeField] private float spawnRadius = 22.0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = -8.0f;
        GameObject spawnCapsule = Instantiate(spawnCapsulePrefab, spawnPosition, Quaternion.identity);
        spawnCapsule.GetComponent<EnemySpawnerCapsule>().enemyPrefab = enemyPrefabs[enemyIndex];
    }

    void FixedUpdate()
    {
        if (spawnTimer < spawnInterval)
        {
            spawnTimer += Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnTimer = 0.0f;
            spawnInterval -= 0.2f;

            if (spawnInterval < 1.0f)
            {
                spawnInterval = 1.0f;
            }
        }
    }
}
