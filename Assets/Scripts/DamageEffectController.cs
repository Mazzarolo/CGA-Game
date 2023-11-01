using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectController : MonoBehaviour
{
    public GameObject damageEffectPrefab;

    public Transform target;

    public float duration;

    public void SpawnDamageEffect(float damage, Vector3 position)
    {
        GameObject damageEffect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        damageEffect.GetComponent<DamageEffectStatus>().Initiate(target, duration, damage, position);
    }
}
