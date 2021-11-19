using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActive : MonoBehaviour
{
    Vector3 mDelta;
    Vector3 mOld;
    Vector3 mNow;

    GameObject cam;

    GameObject active;
    List<GameObject> rings;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        rings = cam.GetComponent<CreateTransformationGizmos>().rings;
    }

    private void Update()
    {
        RotateActiveObject();
    }


    void RotateActiveObject()
    {
        active = GameObject.FindGameObjectWithTag("Active");
        Vector3 unifiedSize = cam.GetComponent<SelectTool>().unifiedSize;
        if (active != null)
        {
            for (int i = 0; i < rings.Count; i++)
            {
                rings[i].transform.position = active.transform.position;
                rings[i].transform.localScale = unifiedSize;
            }
        }
    }


    private void OnMouseDown()
    {
        tag = this.gameObject.tag;
        mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        cam.GetComponent<SelectTool>().fetchTargetSize();
    }

    void OnMouseDrag()
    {
        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mDelta = (mNow - mOld) * 10;

        if (name == "XRot")
        {
            active.transform.Rotate(-mDelta.x, 0, 0);
        }
        else if (name == "YRot")
        {
            active.transform.Rotate(0, -mDelta.x, 0);
        }
        else if (name == "ZRot")
        {
            active.transform.Rotate(0, 0, -mDelta.x);
        }
        mOld = mNow;
    }
}
