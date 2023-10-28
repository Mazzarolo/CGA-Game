using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 10.0f;

    public float knockback = 10.0f;

    public float knockbackDuration = 0.5f;

    public float knockbackHeight = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
            if (playerMovement.isAttacking)
                other.gameObject.GetComponent<EnemyStatus>().TakeDamage(10.0f);
        }
    }

    void Update()
    {
    }
}
