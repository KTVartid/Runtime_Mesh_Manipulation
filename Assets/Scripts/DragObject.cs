using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class DragObject : MonoBehaviour
{
    public Vector3 mOffset;
    public float mZCoord;

    private MeshStudy mesh;
    public int Index;

    private MeshFilter meshFilter;
    public List<int> pairedVertices = new List<int>();



    public void Init(int Index, MeshStudy mesh)
    {
        this.mesh = mesh;
        this.Index = Index;
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
        mesh.ReDraw();
    }



}
