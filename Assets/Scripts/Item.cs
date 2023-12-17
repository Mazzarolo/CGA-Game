using TMPro;
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
    public RuntimeAnimatorController animator;
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

    public void SetSelectedColor(bool selected)
    {
        Button button = GetComponent<Button>();
        ColorBlock colors = button.colors;

        if (selected)
        {
            colors.normalColor = new Color32(26, 255, 0, 255);
            colors.selectedColor = new Color32(26, 255, 0, 255);
            button.colors = colors;
        }
        else
        {
            colors.normalColor = new Color32(3, 0, 41, 255);
            button.colors = colors;
        }
    }

    private void Update()
    {
        if (owned)
            priceTxt.text = "Owned";
    }
}
