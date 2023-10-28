using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private GameObject weapon = null;

    public GameObject hand;

    public float health = 100.0f;

    public float money = 0.0f;

    private float invincibilityTime = 0.0f;

    public int maxInvincibilityTime = 1;
    public void ChangeWeapon(GameObject newWeapon)
    {
        if (weapon)
            Destroy(weapon);

        weapon = Instantiate(newWeapon, transform.position, transform.rotation);
        weapon.transform.parent = hand.transform;
        weapon.transform.localPosition = weapon.GetComponent<WeaponProperties>().startPos;
        weapon.transform.localRotation = Quaternion.Euler(weapon.GetComponent<WeaponProperties>().startRot);
    }

    public void TakeDamage(float damage)
    {

        if (invincibilityTime >= maxInvincibilityTime)
        {
            health -= damage;
            invincibilityTime = 0.0f;
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (invincibilityTime < maxInvincibilityTime)
            invincibilityTime += Time.deltaTime;
    }
}
