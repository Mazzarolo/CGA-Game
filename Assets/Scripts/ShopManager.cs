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

    void Start()
    {
        UpdateMoney();

        selected = null;
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
        }

        if (selected != item && item.owned)
        {
            player.GetComponent<PlayerStatus>().ChangeWeapon(item.prefab, item.animator);
        }

        selected = item;
    }

    public void UpdateMoney()
    {
        money = player.GetComponent<PlayerStatus>().money;

        coinsTxt.text = "Money: $ " + money.ToString();
    }
}
