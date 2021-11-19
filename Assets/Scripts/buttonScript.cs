using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using System;

public class buttonScript : MonoBehaviour, IPointerDownHandler
{
    bool isClicked = false;
    GameObject god;
    GameObject activeIO;

    List<GameObject> transformationRings;
    List<GameObject> transformationAxles;



    private void Start()
    {
        god = GameObject.Find("Main Camera");
        transformationRings = god.GetComponent<CreateTransformationGizmos>().rings;
        transformationAxles = god.GetComponent<CreateTransformationGizmos>().axles;
    }


    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isClicked = true;
        activeIO = god.GetComponent<SelectTool>().old;
        if (god.GetComponent<SelectTool>().activeIO == null)
        {
            for (int i = 0; i < 3; i++)
            {
                transformationAxles[i].SetActive(false);
                transformationRings[i].SetActive(false);
            }
        }
    }





    public void Activate()
    {
        gameObject.GetComponent<UIIsSelected>().selected = true;
        activeIO.GetComponent<MeshStudy>().turnOffEP();
        god.GetComponent<SelectTool>().fetchTargetSize();
        if (gameObject.name == "Move")
        {
            for (int i = 0; i < transformationAxles.Count; i++)
            {
                transformationAxles[i].SetActive(true);
            }
        }
        else if (gameObject.name == "Rotate")
        {
            for (int i = 0; i < transformationAxles.Count; i++)
            {
                transformationRings[i].SetActive(true);
            }
        }
    }

    public void Deactivate()
    {
        gameObject.GetComponent<UIIsSelected>().selected = false;
        if (god.GetComponent<SelectTool>().oldButton == null)
        {
        activeIO.GetComponent<MeshStudy>().turnOnEP();
        }

        if (gameObject.name == "Move")
        {
            for (int i = 0; i < transformationAxles.Count; i++)
            {
                transformationAxles[i].SetActive(false);
            }
        }
        else if (gameObject.name == "Rotate")
        {
            for (int i = 0; i < transformationAxles.Count; i++)
            {
                transformationRings[i].SetActive(false);
            }
        }
    }
}
