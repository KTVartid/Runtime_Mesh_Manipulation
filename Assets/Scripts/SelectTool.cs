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
    public List<GameObject> buttons = new List<GameObject>();

    GameObject wall;

    public GameObject selectedIO;
    public GameObject activeIO;

    public bool UIClicked;
    public GameObject selectedButton;
    public GameObject activeButton;


    Collider activeCollider;
    Vector3 activeSize;
    public Vector3 unifiedSize;

    List<GameObject> transformationRings;
    List<GameObject> transformationAxles;

    public GameObject blade;

    //buttons
    public GameObject moveButton;
    public GameObject rotateButton;
    public GameObject editMeshButton;
    public GameObject cutButton;

    void Start()
    {
        transformationRings = gameObject.GetComponent<CreateTransformationGizmos>().rings;
        transformationAxles = gameObject.GetComponent<CreateTransformationGizmos>().axles;

        wall = GameObject.Find("Wall");
        objects = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            checkClick();
            if (UIClicked && selectedButton != null && selectedButton.GetComponent<UIIsSelected>().selected == false)
            {
                if (activeIO != null)
                {
                activeButton = selectedButton;
                }
                SwitchButtonState();
                ActivateButton();
            }
            else if (UIClicked && selectedButton != null && selectedButton.GetComponent<UIIsSelected>().selected == true)
            {
                DeactivateButtons();
                activeButton = null;
            }
            else if (UIClicked && selectedButton == null)
            {
            }
        }
    }

    void checkClick()
    {
        if (selectedIO != null)
        {
            if (selectedIO.name != "Wall")
            {
                if (activeButton != cutButton)
                {
                    selectedIO.GetComponent<Activator>().Activate();
                }

                if (activeIO != null)
                {
                    activeIO.GetComponent<Activator>().deActivate();
                    activeIO = selectedIO;
                }
                else
                {
                    activeIO = selectedIO;
                }
            }
            else if (selectedIO.name == "Wall" && activeButton == cutButton)
            {

            }
            else if (selectedIO.name == "Wall" && activeIO != null && UIClicked != true)
            {
                activeIO.GetComponent<Activator>().deActivate();
                activeIO = null;
            }
        }
        objects.Clear();
    }

    public void fetchTargetSize()
    {
        if (activeIO != null)
        {
            activeCollider = activeIO.transform.GetComponent<Collider>();
            activeSize = activeCollider.bounds.size;

            float maxSize = Math.Max(Math.Max(activeSize.x, activeSize.y), activeSize.z);
            float averageSize = (activeSize.x + activeSize.y + activeSize.z) / 3f;
            unifiedSize = new Vector3(maxSize, maxSize, maxSize);
        }
    }

    private void OnMouseUp()
    {
        fetchTargetSize();
    }

    public void ActivateButton()
    {
        if (activeIO != null)
        {

            fetchTargetSize();

            if (activeButton.name == "Move")
            {
                DeactivateRotate();
                DeactivateEditMesh();
                DeactivateCut();
                ActivateMove();
            }
            else if (activeButton.name == "Rotate")
            {
                DeactivateMove();
                DeactivateEditMesh();
                DeactivateCut();
                ActivateRotate();
            }
            else if (activeButton.name == "EP")
            {
                DeactivateMove();
                DeactivateRotate();
                DeactivateCut();
                ActivateEditMesh();
            }
            else if (activeButton.name == "Cut")
            {
                DeactivateMove();
                DeactivateRotate();
                DeactivateEditMesh();
                ActivateCut();
            }
        }
    }

    public void DeactivateButtons()
    {
        DeactivateMove();
        DeactivateRotate();
        DeactivateEditMesh();
        DeactivateCut();
    }

    void SwitchButtonState()
    {
        if (selectedButton.GetComponent<UIIsSelected>().selected == true && activeIO != null)
        {
            selectedButton.GetComponent<UIIsSelected>().selected = false;
        }
        else if (selectedButton.GetComponent<UIIsSelected>().selected == false && activeIO != null)
        {
            selectedButton.GetComponent<UIIsSelected>().selected = true;
        }
    }

    public void ActivateMove()
    {
        for (int i = 0; i < transformationAxles.Count; i++)
        {
            transformationAxles[i].SetActive(true);
        }
    }

    public void ActivateRotate()
    {
        for (int i = 0; i < transformationAxles.Count; i++)
        {
            transformationRings[i].SetActive(true);
        }
    }

    public void ActivateEditMesh()
    {
        activeIO.GetComponent<MeshStudy>().turnOnEP();
    }

    public void ActivateCut()
    {
        activeIO.GetComponent<Activator>().deActivate();
        blade.SetActive(true);
        blade.tag = "Active";
        for (int i = 0; i < 2; i++)
        {
            transformationAxles[i].SetActive(true);
        }

        for (int i = 0; i < 3; i++)
        {
            transformationRings[i].SetActive(true);
        }
    }

    public void DeactivateMove()
    {
        for (int i = 0; i < transformationAxles.Count; i++)
        {
            transformationAxles[i].SetActive(false);
        }
        moveButton.GetComponent<UIIsSelected>().selected = false;
    }

    public void DeactivateRotate()
    {
        for (int i = 0; i < transformationAxles.Count; i++)
        {
            transformationRings[i].SetActive(false);
        }
        rotateButton.GetComponent<UIIsSelected>().selected = false;
    }

    public void DeactivateEditMesh()
    {
        activeIO.GetComponent<MeshStudy>().turnOffEP();
        editMeshButton.GetComponent<UIIsSelected>().selected = false;
    }

    public void DeactivateCut()
    {
        blade.SetActive(false);
        blade.tag = "Untagged";
        for (int i = 0; i < 3; i++)
        {
            transformationAxles[i].SetActive(false);
            transformationRings[i].SetActive(false);
        }
        cutButton.GetComponent<UIIsSelected>().selected = false;
    }
}
