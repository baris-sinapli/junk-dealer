using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationScene : MonoBehaviour
{
    private GameObject activeJunk;

    public GameObject ActiveJunk { get => activeJunk; set => activeJunk = value; }

    void Start()
    {
        string junkName = PlayerPrefs.GetString("JunkName");
        Debug.Log(junkName);
        ActiveJunk = transform.Find(junkName).gameObject;
        ActiveJunk.SetActive(true);
    }
}
