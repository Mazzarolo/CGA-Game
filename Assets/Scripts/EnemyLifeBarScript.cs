using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeBarScript : MonoBehaviour
{
    private GameObject target;

    private GameObject lifeBar, redLifeBar;

    private GameObject enemy;

    private Vector3 lifeBarSize;

    void Start()
    {
        enemy = transform.parent.gameObject;
        target = GameObject.Find("Main Camera");
        lifeBar = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        redLifeBar = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        lifeBarSize = lifeBar.GetComponent<Transform>().localScale;
    }

    public void TakeDamage(float damage)
    {
        lifeBar.GetComponent<Transform>().localScale -= new Vector3((lifeBarSize.x * damage) / enemy.GetComponent<EnemyStatus>().maxHealth, 0, 0);
        lifeBar.GetComponent<Transform>().localPosition += new Vector3((lifeBarSize.x * damage) / (enemy.GetComponent<EnemyStatus>().maxHealth * 2), 0, 0);
    }

    private void DamageAnimation()
    {
        if (redLifeBar.GetComponent<Transform>().localScale.x > lifeBar.GetComponent<Transform>().localScale.x)
        {
            redLifeBar.GetComponent<Transform>().localScale -= new Vector3(0.01f, 0, 0);
            redLifeBar.GetComponent<Transform>().localPosition += new Vector3(0.005f, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if(target)
        {
            transform.LookAt(target.transform);
        }

        DamageAnimation();
    }
}
