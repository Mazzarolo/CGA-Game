using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{
    private float money;

    public TextMeshProUGUI coinsTxt;

    public GameObject eventSystem;

    public GameObject player;

    public Item selected;

    private RawImage weaponImage;

    private ArrayList ownedWeapons = new ArrayList();

    void Start()
    {
        UpdateMoney();

        selected = null;

        weaponImage = GameObject.Find("InventoryWeaponSprite").GetComponent<RawImage>();
    }

    public void Buy()
    {
        money = player.GetComponent<PlayerStatus>().money;

        GameObject buttonRef = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;

        Item item = buttonRef.GetComponent<Item>();

        if (money >= item.itemPrice && !item.owned)
        {
            money -= item.itemPrice;
            coinsTxt.text = "Money: $ " + money.ToString();

            item.owned = true;

            player.GetComponent<PlayerStatus>().UpdateMoney((int) money);
            ownedWeapons.Add(item);
        }

        if (selected != item && item.owned)
        {
            player.GetComponent<PlayerStatus>().ChangeWeapon(item.prefab, item.animator);

            if(selected)
                selected.SetSelectedColor(false);

            selected = item;
            selected.SetSelectedColor(true);

            weaponImage.texture = selected.GetComponentInChildren<RawImage>().texture;
            weaponImage.color = weaponImage.color + new Color(0, 0, 0, 1);
        }
    }

    private void ChangeWeaponByScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selected)
            {
                int index = ownedWeapons.IndexOf(selected);

                if (index == ownedWeapons.Count - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }

                selected.SetSelectedColor(false);

                selected = (Item)ownedWeapons[index];

                selected.SetSelectedColor(true);

                player.GetComponent<PlayerStatus>().ChangeWeapon(selected.prefab, selected.animator);

                weaponImage.texture = selected.GetComponentInChildren<RawImage>().texture;
                weaponImage.color = weaponImage.color + new Color(0, 0, 0, 1);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selected)
            {
                int index = ownedWeapons.IndexOf(selected);

                if (index == 0)
                {
                    index = ownedWeapons.Count - 1;
                }
                else
                {
                    index--;
                }

                selected.SetSelectedColor(false);

                selected = (Item)ownedWeapons[index];

                selected.SetSelectedColor(true);

                player.GetComponent<PlayerStatus>().ChangeWeapon(selected.prefab, selected.animator);

                weaponImage.texture = selected.GetComponentInChildren<RawImage>().texture;
                weaponImage.color = weaponImage.color + new Color(0, 0, 0, 1);
            }
        }
    }

    void Update()
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if(!pm.isAttacking && !pm.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !pm.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            ChangeWeaponByScroll();
    }

    public void UpdateMoney()
    {
        money = player.GetComponent<PlayerStatus>().money;

        coinsTxt.text = "Money: $ " + money.ToString();
    }
}
