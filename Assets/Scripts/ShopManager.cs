using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{
    public float money;

    public TextMeshProUGUI coinsTxt;

    public GameObject eventSystem;

    public GameObject player;

    public Item selected;

    void Start()
    {
        coinsTxt.text = "Money: $ " + money.ToString();

        selected = null;
    }

    public void Buy()
    {
        GameObject buttonRef = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;

        Item item = buttonRef.GetComponent<Item>();

        if (money >= item.itemPrice && !item.owned)
        {
            money -= item.itemPrice;
            coinsTxt.text = "Money: $ " + money.ToString();

            item.owned = true;
        }

        if (selected != item && item.owned)
        {
            player.GetComponent<PlayerStatus>().ChangeWeapon(item.prefab);
        }

        selected = item;
    }
}
