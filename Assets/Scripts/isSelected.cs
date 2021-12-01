using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isSelected : MonoBehaviour
{
    public bool selected = false;
    GameObject god;

    public event EventHandler OnClicked;

    private void Start()
    {
        god = GameObject.Find("Main Camera");
    }

    private void OnMouseDown()
    {
        if (gameObject.tag != "Active")
        {
            selected = true;
            god.GetComponent<SelectTool>().selectedIO = gameObject;
        }
    }

    private void OnMouseUp()
    {
        god.GetComponent<SelectTool>().selectedIO = null;
        selected = false;
    }
}
