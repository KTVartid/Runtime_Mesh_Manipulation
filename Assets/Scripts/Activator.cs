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
        if (god.GetComponent<SelectTool>().oldButton == null)
        {
            gameObject.GetComponent<MeshStudy>().turnOnEP();
        }
    }
    public void deActivate()
    {
        myRend.material = inactive;
        gameObject.tag = ("IO");
        gameObject.GetComponent<MeshStudy>().turnOffEP();
    }
}
