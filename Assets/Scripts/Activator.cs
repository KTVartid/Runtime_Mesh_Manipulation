using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private Material inactive;
    private Material active;
    private MeshRenderer myRend;

    GameObject god;

    void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        active = Resources.Load("mesh", typeof(Material)) as Material;
        inactive = Resources.Load("unSelected", typeof(Material)) as Material;
        god = GameObject.Find("Main Camera");
    }

    public void Activate()
    {
        myRend.material = active;
        transform.tag = "Active";
        if (god.GetComponent<SelectTool>().activeButton != null && god.GetComponent<SelectTool>().activeButton.GetComponent<UIIsSelected>().selected == true)
        {
            if (god.GetComponent<SelectTool>().activeButton.name == "EP")
            {
                gameObject.GetComponent<MeshStudy>().turnOnEP();
            }
            else if (god.GetComponent<SelectTool>().activeButton.name == "Move")
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject axle = god.GetComponent<CreateTransformationGizmos>().axles[i];
                    axle.SetActive(true);
                }
            }
            else if (god.GetComponent<SelectTool>().activeButton.name == "Rotate")
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject ring = god.GetComponent<CreateTransformationGizmos>().rings[i];
                    ring.SetActive(true);
                }
            }
        }
    }
    public void deActivate()
    {
        myRend.material = inactive;
        gameObject.tag = ("IO");
        gameObject.GetComponent<MeshStudy>().turnOffEP();
        if (god.GetComponent<SelectTool>().selectedIO.name == "Wall" && god.GetComponent<SelectTool>().UIClicked == false)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject axle = god.GetComponent<CreateTransformationGizmos>().axles[i];
                GameObject ring = god.GetComponent<CreateTransformationGizmos>().rings[i];
                axle.SetActive(false);
                ring.SetActive(false);
            }
        }
    }
}
