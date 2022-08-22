using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickElement : MonoBehaviour
{
    private GameObject _element;
    private GameObject _lastElement = null;
    private Junk _elementContent;

    [SerializeField] private GameObject Kanvas;
    

    public GameObject Element { get => _element; set => _element = value; }

    private void Start()
    {
        Kanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // To blocking interaction through UI 

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 position = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch began!");
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                { 
                    if(hitInfo.collider.tag == "particle")
                    {
                        Debug.Log("Particle has been touched!");
                        _element = hitInfo.collider.gameObject;

                        _element.GetComponentInParent<SpawnManager>().LastSpawnTime((ulong)DateTime.Now.Ticks);

                        _elementContent = _element.GetComponent<JunkContent>().junkContent;

                        Kanvas.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = _elementContent.junkName;
                        Kanvas.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = _elementContent.junkImage;
                        Kanvas.transform.GetChild(1).GetChild(4).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = _elementContent.baseValue.ToString();
                        Kanvas.SetActive(true);

                    }


                    if (hitInfo.collider.tag == "platform")
                    {
                        if (_lastElement != null && _lastElement.transform.GetChild(0) != null)
                        {
                            _lastElement.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        Debug.Log("Platform "+ hitInfo.collider.name + " has been touched!");
                        _element = hitInfo.collider.gameObject;
                        _lastElement = hitInfo.collider.gameObject;
                        _element.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
