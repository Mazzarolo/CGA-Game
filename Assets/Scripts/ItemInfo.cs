using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI priceTxt;
    public GameObject shopManager;

    public GameObject prefab;

    void Start()
    {
        prefab.layer = 8;
        foreach (Transform child in prefab.transform)
        {
            child.gameObject.layer = 8;
        }
    }

    void Update()
    {
        if (shopManager.GetComponent<ShopManager>().items[itemID].owned)
            priceTxt.text = "Owned";
        else
            priceTxt.text = "$ " + shopManager.GetComponent<ShopManager>().items[itemID].itemPrice.ToString();
        
        nameTxt.text = shopManager.GetComponent<ShopManager>().items[itemID].itemName;

        if (!shopManager.GetComponent<ShopManager>().items[itemID].prefab)
            shopManager.GetComponent<ShopManager>().items[itemID].prefab = prefab;
    }
}
