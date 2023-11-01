using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffectStatus : MonoBehaviour
{
    private Transform target;

    private float duration, timer;
    
    public void Initiate(Transform target, float duration, float damage, Vector3 position)
    {
        this.target = target;
        this.duration = duration;
        transform.position = position;
        GetComponentInChildren<TextMeshPro>().text = damage.ToString();
        timer = 0;
    }

    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
        }

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
