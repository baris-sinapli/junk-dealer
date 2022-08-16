using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{

    public void RestoreButtonPressed()
    {
        Debug.Log("Restored...");
        
        Destroy(Camera.main.GetComponent<PickElement>().Element);
    }

    public void SellButtonPressed()
    {
        Debug.Log("Object selled...");

        Destroy(Camera.main.GetComponent<PickElement>().Element);
    }
}
