using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIsSelected : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    public bool selected;
    GameObject god;



    private void Start()
    {
        god = GameObject.Find("Main Camera");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        god.GetComponent<SelectTool>().UIClicked = true;
        god.GetComponent<SelectTool>().activeButton = gameObject;
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        god.GetComponent<SelectTool>().UIClicked = false;
        god.GetComponent<SelectTool>().activeButton = null;
        god.GetComponent<SelectTool>().fetchTargetSize();
    }


}
