using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public bool isUnlocked = false;
    public Junk[] collectableJunks;
    public float salePrice;
    [SerializeField] private TextMeshProUGUI ticketTMP;
    [SerializeField] private TextMeshProUGUI currentMoneyTMP;

    private void Awake()
    {
        int isActive = PlayerPrefs.GetInt(transform.name + ".isActive");
        if(isActive == 1)
        {
            isUnlocked = true;
            UnlockPlatform();
        }
    }

    private void Update()
    {
        if (ticketTMP != null)
        {
            ticketTMP.text = "BUY\n$" + salePrice.ToString();

            if (salePrice > float.Parse(currentMoneyTMP.text))
            {
                Debug.Log(float.Parse(currentMoneyTMP.text));
                ticketTMP.color = Color.red;
            }
            else
            {
                ticketTMP.color = Color.green;
            }
        }
    }

    public void UnlockPlatform()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        isUnlocked = true;
    }
}
