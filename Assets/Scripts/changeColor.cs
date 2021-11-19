using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    public Color startColor;
    public Color mouseOverColor;
    public Color mouseClickColor;
    bool mouseOver = false;

    void Start()
    {
        startColor = GetComponent<Renderer>().material.color;
        mouseOverColor = Color.yellow;
        mouseClickColor = Color.white;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) == false)
        {
        mouseOver = true;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
        }
    }

    private void OnMouseDown()
    {
        if (mouseOver == true)
        {
            GetComponent<Renderer>().material.SetColor("_Color", mouseClickColor);
        }
    }

    private void OnMouseUp()
    {
        if (mouseOver == true)
        {
            GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
        }
            GetComponent<Renderer>().material.SetColor("_Color", startColor);

    }

    private void OnMouseExit()
    {
        if (Input.GetMouseButton(0) == false)
        { 
            mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
        }
    }
}
