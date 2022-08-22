using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public bool isUnlocked = false;
    public Junk[] collectableJunks;
    public float salePrice;

    private void Awake()
    {
        int isActive = PlayerPrefs.GetInt(transform.name + ".isActive");
        if(isActive == 1)
        {
            isUnlocked = true;
            UnlockPlatform();
        }
    }

    public void UnlockPlatform()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        isUnlocked = true;
    }
}
