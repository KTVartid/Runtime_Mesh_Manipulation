using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SelectTool : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickablesLayer;

    [SerializeField]
    private Material unselected;

    List<GameObject> objects;
    List<GameObject> buttons;

    GameObject wall;

    public GameObject activeIO;
    public GameObject old;

    public bool UIClicked;
    public GameObject activeButton;
    public GameObject oldButton;

    Collider activeCollider;
    Vector3 activeSize;
    public Vector3 unifiedSize;


    private void Start()
    {
        wall = GameObject.Find("Wall");
        objects = new List<GameObject>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            checkClick();
            activateButton();
        }
    }

    void checkClick()
    {
        if (activeIO != null)
        {
            if (activeIO.name != "Wall")
            {
                activeIO.GetComponent<Activator>().Activate();
                if (old != null)
                {
                    old.GetComponent<Activator>().deActivate();
                    old = activeIO;
                }
                else
                {
                    old = activeIO;
                }
            }
            else if (activeIO.name == "Wall" && old != null && UIClicked != true)
            {
                old.GetComponent<Activator>().deActivate();
                old = null;
            }
        }
        objects.Clear();
    }

    void activateButton()
    {
        if (activeButton != null && activeIO != null)
        {
            activeButton.GetComponent<buttonScript>().Activate();
            if (oldButton != null)
            {
                oldButton.GetComponent<buttonScript>().Deactivate();
                if (oldButton == activeButton)
                {
                    oldButton = null;
                }
                else
                {
                    oldButton = activeButton;
                }
            }
            else if(activeButton != null)
            {
                oldButton = activeButton;
            }
        }
    }

    public void fetchTargetSize()
    {
        activeCollider = old.transform.GetComponent<Collider>();
        activeSize = activeCollider.bounds.size;

        float maxSize = Math.Max(Math.Max(activeSize.x, activeSize.y), activeSize.z);
        float averageSize = (activeSize.x + activeSize.y + activeSize.z) / 3f;
        unifiedSize = new Vector3(maxSize, maxSize, maxSize);
    }

    private void OnMouseUp()
    {
        fetchTargetSize();
    }
}
