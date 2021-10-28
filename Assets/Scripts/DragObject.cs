using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class DragObject : MonoBehaviour
{
    public Vector3 mOffset;
    public float mZCoord;
    Vector3 mDelta;
    public Vector3 mOld;
    public Vector3 mNow;

    public MeshStudy mesh;
    public int Index;

    public List<int> pairedVertices = new List<int>();
    public List<GameObject> connectedEP = new List<GameObject>();
    public List<GameObject> EPList = new List<GameObject>();

    public GameObject EPParent;
    Vector3[] parentVertices;

    private void Start()
    {
        parentVertices = mesh.vertices;
    }

    public void Init(int Index, MeshStudy mesh)
    {
        this.mesh = mesh;
        this.Index = Index;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mOld = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            mNow = transform.position;
            mDelta = mNow - mOld;
            mOld = mNow;
        }
    }

    private void OnMouseDown()
    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    public Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x, y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    void OnMouseDrag()
    {


        transform.position = GetMouseWorldPos() + mOffset;
        for (int i = 0; i < pairedVertices.Count; i++)
        {
            mesh.vertices[pairedVertices[i]] = transform.localPosition;
        }






        for (int i = 0; i < connectedEP.Count; i++)
        {
            connectedEP[i].transform.localPosition -= (-mDelta / 10);
        }

        mesh.ReDraw();
    }



}
