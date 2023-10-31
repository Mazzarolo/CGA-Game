using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public bool owned;
    public string itemName;
    public float itemPrice;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI priceTxt;
    public GameObject shopManager;
    public GameObject prefab;
    public AnimatorController animator;
    private void Start()
    {
        owned = false;

        prefab.layer = 8;
        
        foreach (Transform child in prefab.transform)
        {
            child.gameObject.layer = 8;
        }

        nameTxt.text = itemName;

        priceTxt.text = "$ " + itemPrice.ToString();
    }

    private void Update()
    {
        if (owned)
            priceTxt.text = "Owned";
    }
}
