using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 10.0f;

    public float knockback;

    public float knockbackHeight;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
            Vector3 dir = other.gameObject.transform.position - transform.position;
            if (playerMovement.isAttacking)
                other.gameObject.GetComponentInParent<EnemyStatus>().TakeDamage(damage, knockback, new Vector3(dir.x, knockbackHeight, dir.z).normalized);
        }
    }

    void Update()
    {
    }
}
