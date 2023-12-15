using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private float damage, knockback, knockbackHeight;

    private EnemyStatus enemyStatus;

    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyStatus = GetComponentInParent<EnemyStatus>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        damage = enemyStatus.damage;
        knockback = enemyStatus.knockback;
        knockbackHeight = enemyStatus.knockbackHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && enemyMovement.isAttacking && enemyMovement.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") 
            && enemyMovement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
        {
            Vector3 dir = other.gameObject.transform.position - transform.position;
            other.gameObject.GetComponentInParent<PlayerStatus>().TakeDamage(damage, knockback, new Vector3(dir.x, knockbackHeight, dir.z).normalized);
        }
    }
}
