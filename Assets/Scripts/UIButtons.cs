using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameObject Kanvas;
    [SerializeField] private GameObject PlatformManager;
    [SerializeField] private TextMeshProUGUI CurrentMoneyTMP;
    [SerializeField] private TextMeshProUGUI SellPriceTMP;
    
    
    public void RestoreButtonPressed()
    {
        Debug.Log("Restored...");

        PickElement pickElement = Camera.main.GetComponent<PickElement>();
        // Save picked element's junk content to carry restoration scene
        string junkName = pickElement.Element.GetComponent<JunkContent>().junkContent.junkName;
        Debug.Log(junkName);
        float baseVal = pickElement.Element.GetComponent<JunkContent>().junkContent.baseValue;
        Debug.Log(baseVal);
        PlayerPrefs.SetString("JunkName", junkName);
        PlayerPrefs.SetFloat("BaseValue", baseVal);
        PlayerPrefs.Save();

        Destroy(pickElement.Element);

        SceneManager.LoadScene("Restoration");
        
    }

    public void SellButtonPressed()
    {
        Debug.Log("Object selled...");
        float currentMoney = float.Parse(CurrentMoneyTMP.text);
        float sellPrice = float.Parse(SellPriceTMP.text);
        CurrentMoneyTMP.text = (currentMoney + sellPrice).ToString("0000");

        PlayerPrefs.SetFloat("MoneyAmount", currentMoney + sellPrice);

        Kanvas.SetActive(false);
        Destroy(Camera.main.GetComponent<PickElement>().Element);

    }

    public void BuyPlatformButtonPressed()
    {

        float currentMoney = float.Parse(CurrentMoneyTMP.text);
        float salePrice = PlatformManager.GetComponent<PlatformManager>().salePrice;
        if (currentMoney >= salePrice)
        {
            PlatformManager.GetComponent<PlatformManager>().UnlockPlatform();
            CurrentMoneyTMP.text = (currentMoney - salePrice).ToString("0000");
            PlayerPrefs.SetFloat("MoneyAmount", currentMoney - salePrice);

            // Save unlocked information of platform_name
            bool isPlatformUnlocked = PlatformManager.GetComponent<PlatformManager>().isUnlocked;
            if(isPlatformUnlocked == true)
            {
                PlayerPrefs.SetInt(PlatformManager.name + ".isActive", 1);
            }
            else
            {
                PlayerPrefs.SetInt(PlatformManager.name + ".isActive", 0);
            }
            
        }

    }

    public void SellRestoredButton()
    {
        Debug.Log("Object selled after restoring...");
        float sellPrice = float.Parse(SellPriceTMP.text);
        float currentMoney = PlayerPrefs.GetFloat("MoneyAmount");
        PlayerPrefs.SetFloat("MoneyAmount", currentMoney + sellPrice);
        SceneManager.LoadScene("Main");
    }
}
