using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIsSelected : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    public bool selected;
    GameObject god;
    GameObject activeIO;



    private void Start()
    {
        god = GameObject.Find("Main Camera");
        activeIO = god.GetComponent<SelectTool>().activeIO;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        god.GetComponent<SelectTool>().UIClicked = true;
        god.GetComponent<SelectTool>().selectedButton = gameObject;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        god.GetComponent<SelectTool>().UIClicked = false;
        god.GetComponent<SelectTool>().selectedButton = null;
        god.GetComponent<SelectTool>().fetchTargetSize();
    }

}
