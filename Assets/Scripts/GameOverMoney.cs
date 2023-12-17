using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMoney : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        moneyText.text = "$ " + PlayerStatus.earnedMoney.ToString();
    }
}
