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
        Destroy(Camera.main.GetComponent<PickElement>().Element);
        SceneManager.LoadScene("Restoration");
        
    }

    public void SellButtonPressed()
    {
        Debug.Log("Object selled...");
        float currentMoney = float.Parse(CurrentMoneyTMP.text);
        float sellPrice = float.Parse(SellPriceTMP.text);
        CurrentMoneyTMP.text = (currentMoney + sellPrice).ToString("0000");
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
        }

    }

    public void SellRestoredButton()
    {
        Debug.Log("Object selled after restoring...");
        SceneManager.LoadScene("Main");
    }
}
