using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float health = 100.0f;

    private float invincibilityTime = 0.0f;

    public int maxInvincibilityTime = 1;

    public float knockback;

    public void TakeDamage(float damage, float knockback, Vector3 knockbackDir)
    {
        if (invincibilityTime >= maxInvincibilityTime)
        {
            health -= damage;
            invincibilityTime = 0.0f;
            GetComponent<Rigidbody>().AddForce(knockbackDir * knockback);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 dir = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<PlayerStatus>().TakeDamage(10.0f, knockback, new Vector3(dir.x, 0, dir.z).normalized);
        }
    }

    void Update()
    {
        if (health <= 0.0f)
            Die();
    }

    void FixedUpdate()
    {
        if (invincibilityTime < maxInvincibilityTime)
            invincibilityTime += Time.deltaTime;
    }
}
