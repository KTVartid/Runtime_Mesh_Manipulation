using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour
{
    [SerializeField]
    private Material selected;
    [SerializeField]
    private Material unselected;

    private MeshRenderer myRend;

    void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        selected = Resources.Load("mesh", typeof(Material)) as Material;
        unselected = Resources.Load("unSelected", typeof(Material)) as Material;
    }

    public void Activate()
    {
        myRend.material = selected;
        transform.tag = "Active";

        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}
