using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameObject Kanvas;
    public void RestoreButtonPressed()
    {
        Debug.Log("Restored...");
        SceneManager.LoadScene("Restoration");

        Destroy(Camera.main.GetComponent<PickElement>().Element);
    }

    public void SellButtonPressed()
    {
        Debug.Log("Object selled...");
        Kanvas.SetActive(false);
        Destroy(Camera.main.GetComponent<PickElement>().Element);
    }

    public void BuyPlatformButtonPressed()
    {
        transform.parent.parent.gameObject.SetActive(false);

    }

    public void SellRestoredButton()
    {
        SceneManager.LoadScene("Main");
    }
}
