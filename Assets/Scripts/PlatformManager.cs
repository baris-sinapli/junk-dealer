using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public bool isUnlocked = false;
    public Junk[] collectableJunks;
    public float salePrice;

    public void UnlockPlatform()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        isUnlocked = true;
    }
}
