using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI MoneyText = transform.GetComponent<TextMeshProUGUI>();
        MoneyText.text = PlayerPrefs.GetFloat("MoneyAmount").ToString("0000");
    }
}
