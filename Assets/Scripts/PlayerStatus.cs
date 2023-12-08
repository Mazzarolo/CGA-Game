using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject redLifeBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private TextMeshProUGUI moneyText;

    private GameObject weapon = null;

    public GameObject hand;

    public float health = 100.0f;

    public int money;

    private float invincibilityTime = 0.0f;

    public int maxInvincibilityTime = 1;

    private Vector3 lifeBarSize;

    void Awake()
    {
        lifeBarSize = lifeBar.GetComponent<Transform>().localScale;

        healthText.text = health.ToString() + "/100";

        moneyText.text = money.ToString();
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
            healthText.text = health.ToString() + "/100";
            GetComponent<Rigidbody>().AddForce(knockbackDir * knockback);
        }
    }

    public void Heal(float heal)
    {
        if (health < 100.0f)
        {
            if(health + heal > 100.0f)
            {
                heal = 100.0f - health;
            }
            health += heal;
            lifeBar.GetComponent<Transform>().localScale += new Vector3(lifeBarSize.x * heal / 100, 0, 0);
            lifeBar.GetComponent<Transform>().localPosition += new Vector3(lifeBarSize.x * heal / 200, 0, 0);
            healthText.text = health.ToString() + "/100";
        }
    }

    private void DamageBarAnimation ()
    {
        if (redLifeBar.GetComponent<Transform>().localScale.x >= lifeBar.GetComponent<Transform>().localScale.x)
        {
            redLifeBar.GetComponent<Transform>().localScale -= new Vector3(0.005f, 0, 0);
        }
    }

    public void UpdateMoney(int money)
    {
        this.money = money;
        moneyText.text = money.ToString();
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
