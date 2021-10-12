using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickablesLayer;

    [SerializeField]
    private Material unselected;


    List<GameObject> objects;

    private void Start()
    {
        objects = new List<GameObject>();
    }


    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            Deactivate();

            RaycastHit rayHit;

            //check if click hit anything
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, clickablesLayer))
            {
                rayHit.collider.GetComponent<ClickOn>().Activate();
            }

        }

        objects.Clear();
    }

    private void Deactivate()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Active"))
        {
            obj.transform.tag = "IO";
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("IO"))
        {
            objects.Add(obj);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].GetComponent<MeshRenderer>().material = unselected;
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EP"))
        {
            obj.SetActive(false);
        }

    }
}
