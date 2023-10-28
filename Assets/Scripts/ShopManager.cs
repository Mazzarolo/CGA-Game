using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aux_Classes;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{

    public Item[] items = new Item[1];

    public float money;

    public TextMeshProUGUI coinsTxt;

    public GameObject eventSystem;

    void Start()
    {
        coinsTxt.text = "Money: R$ " + money.ToString();

        items[0] = new Item(0, "Baseball Bat", 10.0f);
    }

    public void Buy()
    {
        GameObject buttonRef = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;

        int itemID = buttonRef.GetComponent<ItemInfo>().itemID;

        if (money >= items[itemID].itemPrice && !items[itemID].owned)
        {
            money -= items[itemID].itemPrice;
            coinsTxt.text = "Money: R$ " + money.ToString();
            items[itemID].owned = true;
        }
    }
}
