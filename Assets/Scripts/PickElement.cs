using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickElement : MonoBehaviour
{
    private GameObject _element;
    private GameObject _lastElement = null;
    [SerializeField] private GameObject Kanvas;

    public GameObject Element { get => _element; set => _element = value; }

    private void Start()
    {
        Kanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
