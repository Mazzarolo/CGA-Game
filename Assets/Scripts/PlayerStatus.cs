using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject redLifeBar;
    [SerializeField] private TextMeshProUGUI healthText;
    public RawImage potionImage;
    public TextMeshProUGUI numPotText;

    [SerializeField] private TextMeshProUGUI moneyText;

    private GameObject weapon = null;

    public GameObject hand;

    public GameObject leftHand;

    public float health = 100.0f;

    public int money;

    private float invincibilityTime = 0.0f;

    public int maxInvincibilityTime = 1;

    private Vector3 lifeBarSize;

    private GameObject potion = null;

    public int numPot = 0;

    private float remainHeal;

    void Awake()
    {
        lifeBarSize = lifeBar.GetComponent<Transform>().localScale;

        healthText.text = health.ToString() + "/100";

        moneyText.text = money.ToString();

        remainHeal = 0.0f;
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
        numPot--;

        if (numPot == 0)
        {
            numPotText.color = new Color32(255, 255, 255, 0);
            potionImage.color = new Color32(255, 255, 255, 0);
        }

        if(health + heal > 100.0f)
        {
            heal = 100.0f - health;
        }
        health += heal;
        
        remainHeal = heal;

        healthText.text = health.ToString() + "/100";
    }

    public void InstantiatePotion(GameObject potion)
    {
        if (!this.potion)
        {
            this.potion = Instantiate(potion, transform.position, transform.rotation);
            this.potion.transform.parent = leftHand.transform;
            this.potion.transform.localPosition = new Vector3(0.0f, 0.05f, -0.2f);
            this.potion.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, -45.0f));
        }
    }

    public void DestroyPotion()
    {
        if (potion)
        {
            Destroy(potion);
            potion = null;
        }
    }

    private void DamageBarAnimation ()
    {
        if (redLifeBar.GetComponent<Transform>().localScale.x > lifeBar.GetComponent<Transform>().localScale.x)
        {
            redLifeBar.GetComponent<Transform>().localScale -= new Vector3(0.005f, 0, 0);
        }
        else if (redLifeBar.GetComponent<Transform>().localScale.x < lifeBar.GetComponent<Transform>().localScale.x)
        {
            redLifeBar.GetComponent<Transform>().localScale = lifeBar.GetComponent<Transform>().localScale;
        }
    }

    private void HealAnimation ()
    {
        if (remainHeal > 0.0f)
        {
            float healing = remainHeal / 40.0f;

            lifeBar.GetComponent<Transform>().localScale += new Vector3(lifeBarSize.x * healing / 100, 0, 0);
            lifeBar.GetComponent<Transform>().localPosition += new Vector3(lifeBarSize.x * healing / 200, 0, 0);

            remainHeal -= healing;
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
        HealAnimation();

        if (invincibilityTime < maxInvincibilityTime)
            invincibilityTime += Time.deltaTime;
    }

    void Update()
    {
        numPotText.text = numPot.ToString();
        Respawn();
    }
}
