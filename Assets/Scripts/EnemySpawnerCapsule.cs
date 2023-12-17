using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerCapsule : MonoBehaviour
{
    public GameObject enemyPrefab;

    private float spawnHeight = -4.0f, destroyheight = -8.0f;

    private bool spawned = false;

    private float speed = 0.05f;

    void MoveCapsule()
    {
        if (spawned == false && transform.position.y < spawnHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
        else if (spawned == true && transform.position.y > destroyheight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        else if (spawned == true && transform.position.y <= destroyheight)
        {
            if (enemyPrefab != null)
                if (enemyPrefab.GetComponent<EnemyMovement>() != null)
                    enemyPrefab.GetComponent<EnemyMovement>().isSpawning = false;
            Destroy(gameObject);
        }
    }

    void SpawnEnemy()
    {
        if (spawned == false && transform.position.y >= spawnHeight)
        {
            enemyPrefab = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            if (enemyPrefab.GetComponent<EnemyMovement>() != null)
            {
                enemyPrefab.GetComponent<EnemyMovement>().isSpawning = true;
            }
            spawned = true;
        }
    }

    void FixedUpdate()
    {
        MoveCapsule();
        SpawnEnemy();
    }
}
