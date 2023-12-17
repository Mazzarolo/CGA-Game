using UnityEngine;

public class EnemySpawnerCapsule : MonoBehaviour
{
    public GameObject enemyPrefab;

    private float spawnHeight = -4.0f, destroyheight = -8.0f;

    private bool spawned = false;

    private float speed = 0.05f;

    private float addLife = 0.0f;

    private float addValue = 0.0f;

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
            enemyPrefab.GetComponent<EnemyStatus>().maxHealth += addLife;
            enemyPrefab.GetComponent<EnemyStatus>().health += addLife;
            enemyPrefab.GetComponent<EnemyStatus>().value += addValue;
            spawned = true;
        }
    }

    void FixedUpdate()
    {
        MoveCapsule();
        SpawnEnemy();

        float playerMoney = PlayerStatus.earnedMoney;

        addLife = (float) System.Math.Floor(playerMoney / 100.0f) * 5.0f;

        addValue = (float) System.Math.Floor(playerMoney / 100.0f) * 15.0f;
    }
}
