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

    private MeshStudy mesh;
    public int Index;

    private MeshFilter meshFilter;
    private Mesh parentMesh;
    public List<int> pairedVertices = new List<int>();
    public List<int> connectedVertices = new List<int>();
    public List<GameObject> connectedEP = new List<GameObject>();
    public List<GameObject> EPList = new List<GameObject>();


    public GameObject EPParent;
    int[] parentTriangles;
    Vector3[] parentVertices;

    private void Start()
    {
        EPParent = transform.parent.gameObject;
        meshFilter = EPParent.GetComponent<MeshFilter>();
        parentMesh = meshFilter.sharedMesh;
        EPList = EPParent.GetComponent<MeshStudy>().EPList;
        parentVertices = mesh.vertices;
        parentTriangles = mesh.triangles;
        //Init();
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

    //public void Init()
    //{
    //    for (int i = 0; i < pairedVertices.Count; i++)
    //    {
    //        mesh.vertices[pairedVertices[i]] = transform.localPosition;

    //        // find connected vertices
    //        for (int t = 0; t < parentTriangles.Length; t += 3)
    //        {
    //            Vector3 p1 = parentVertices[parentTriangles[t + 0]];
    //            Vector3 p2 = parentVertices[parentTriangles[t + 1]];
    //            Vector3 p3 = parentVertices[parentTriangles[t + 2]];

    //            if (parentVertices[pairedVertices[i]] == p1 /*&& !connectedVertices.Contains(parentTriangles[t + 1]) && !connectedVertices.Contains(parentTriangles[t + 2])*/)
    //            {
    //                connectedVertices.Add(parentTriangles[t + 1]);
    //                connectedVertices.Add(parentTriangles[t + 2]);
    //            }
    //            if (parentVertices[pairedVertices[i]] == p2 /*&& !connectedVertices.Contains(parentTriangles[t + 0]) && !connectedVertices.Contains(parentTriangles[t + 2])*/)
    //            {
    //                connectedVertices.Add(parentTriangles[t + 0]);
    //                connectedVertices.Add(parentTriangles[t + 2]);
    //            }
    //            if (parentVertices[pairedVertices[i]] == p3 /*&& !connectedVertices.Contains(parentTriangles[t + 0]) && !connectedVertices.Contains(parentTriangles[t + 1])*/)
    //            {
    //                connectedVertices.Add(parentTriangles[t + 0]);
    //                connectedVertices.Add(parentTriangles[t + 1]);
    //            }
    //        }
    //    }

    //    // find connected EditorPoints
    //    for (int j = 0; j < EPList.Count; j++)
    //    {
    //        for (int i = 0; i < connectedVertices.Count; i++)
    //        {
    //            if (connectedVertices[i] == j && !connectedEP.Contains(EPList[j]))
    //            {
    //                connectedEP.Add(EPList[j]);
    //            }
    //        }
    //    }
    //}

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
            //for (int j = 0; j < pairedVertices.Count; j++)
            //{
            //    //parentVertices[connectedEP[i].GetComponent<DragObject>().pairedVertices[j]] = connectedEP[i].transform.localPosition;
            //}
        }

        mesh.ReDraw();
    }



}
