using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    public Color startColor;
    public Color mouseOverColor;
    bool mouseOver = false;

    void Start()
    {
        startColor = GetComponent<Renderer>().material.color;
        mouseOverColor = Color.yellow;
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
}
