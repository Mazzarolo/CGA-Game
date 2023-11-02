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
    private TextMeshProUGUI nameTxt;
    private TextMeshProUGUI priceTxt;
    public GameObject prefab;
    public AnimatorController animator;
    private void Start()
    {
        nameTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        priceTxt = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

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
