using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI priceTxt;
    public GameObject shopManager;

    void Update()
    {
        if (shopManager.GetComponent<ShopManager>().items[itemID].owned)
            priceTxt.text = "Owned";
        else
            priceTxt.text = "R$ " + shopManager.GetComponent<ShopManager>().items[itemID].itemPrice.ToString();
        nameTxt.text = shopManager.GetComponent<ShopManager>().items[itemID].itemName;
    }
}
