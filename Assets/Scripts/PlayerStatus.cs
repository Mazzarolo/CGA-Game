using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject redLifeBar;

    private GameObject weapon = null;

    public GameObject hand;

    public float health = 100.0f;

    public float money = 0.0f;

    private float invincibilityTime = 0.0f;

    public int maxInvincibilityTime = 1;

    private Vector3 lifeBarSize;

    void Awake()
    {
        lifeBarSize = lifeBar.GetComponent<Transform>().localScale;
    }
    public void ChangeWeapon(GameObject newWeapon, AnimatorController animator)
    {
        if (weapon)
            Destroy(weapon);

        weapon = Instantiate(newWeapon, transform.position, transform.rotation);
        weapon.transform.parent = hand.transform;
        weapon.transform.localPosition = weapon.GetComponent<WeaponProperties>().startPos;
        weapon.transform.localRotation = Quaternion.Euler(weapon.GetComponent<WeaponProperties>().startRot);
        GetComponentInChildren<Animator>().runtimeAnimatorController = animator;
    }

    public void TakeDamage(float damage, float knockback, Vector3 knockbackDir)
    {
        if (invincibilityTime >= maxInvincibilityTime && health > 0.0f)
        {
            health -= damage;
            invincibilityTime = 0.0f;
            lifeBar.GetComponent<Transform>().localScale -= new Vector3(lifeBarSize.x * damage / 100, 0, 0);
            lifeBar.GetComponent<Transform>().localPosition -= new Vector3(lifeBarSize.x * damage / 200, 0, 0);
            GetComponent<Rigidbody>().AddForce(knockbackDir * knockback);
        }
    }

    private void DamageBarAnimation ()
    {
        if (redLifeBar.GetComponent<Transform>().localScale.x >= lifeBar.GetComponent<Transform>().localScale.x)
        {
            redLifeBar.GetComponent<Transform>().localScale -= new Vector3(lifeBarSize.x / 1000, 0, 0);
        }
    }

    private void Respawn ()
    {
        if (redLifeBar.GetComponent<Transform>().localScale.x <= 0)
            SceneManager.LoadScene("SampleScene");
    }

    void FixedUpdate()
    {
        DamageBarAnimation();

        if (invincibilityTime < maxInvincibilityTime)
            invincibilityTime += Time.deltaTime;
    }

    void Update()
    {
        Respawn();
    }
}
