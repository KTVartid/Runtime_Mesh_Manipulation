using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class DragObject : MonoBehaviour
{
    Vector3 mOffset;
    float mZCoord;
    Vector3 mDelta;
    Vector3 mOld;
    Vector3 mNow;

    MeshStudy mesh;
    public int Index;

    public List<int> pairedVertices = new List<int>();
    public List<GameObject> connectedEP = new List<GameObject>();
    public List<HashSet<GameObject>> level;

    public int sharpness;

    public List<DragObject> siblings = new List<DragObject>();
    public List<int> points = new List<int>();
    public List<int> triangles = new List<int>();


    void Start()
    {
        
    }

    public void Init(int Index, MeshStudy mesh)
    {
        this.mesh = mesh;
        this.Index = Index;
    }

    void Update()
    {
        sharpness = (int)GameObject.Find("Sharpness").GetComponent<Slider>().value;

        //mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mDelta = mNow - mOld;
        mOld = mNow;

        for (int i = 0; i < pairedVertices.Count; i++)
        {
            if (mesh.vertices[pairedVertices[i]] != transform.localPosition)
            {
                mesh.vertices[pairedVertices[i]] = transform.localPosition;
            }
        }
    }

    private void OnMouseDown()
    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();

        level = new List<HashSet<GameObject>>();
        HashSet<GameObject> draggedEP = new HashSet<GameObject>();
        draggedEP.Add(this.gameObject);
        level.Add(draggedEP);        // add first level manualy
        for (int i = 1; i < sharpness; i++)
        {
            foreach (GameObject currentEP in level[i - 1])
            {
                HashSet<GameObject> actualConnectedEPList = new HashSet<GameObject>();
                for (int j = 0; j < currentEP.GetComponent<DragObject>().connectedEP.Count; j++)
                {
                    if (!Check(currentEP.GetComponent<DragObject>().connectedEP[j], level))
                    {
                        actualConnectedEPList.Add(currentEP.GetComponent<DragObject>().connectedEP[j]);
                    }
                }
                level.Add(actualConnectedEPList);
            }
        }
    }

    public Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x, y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    bool Check(GameObject actualConnectedEP, List<HashSet<GameObject>> level)
    {
        for (int i = 0; i < level.Count; i++)
        {
            if (level[i].Contains(actualConnectedEP))
            {
                return true;
            }
        }
        return false;
    }


    void OnMouseDrag()
    {

        for (int i = 0; i < level.Count; i++)
        {
            foreach (GameObject currentEP in level[i])
            {
                currentEP.transform.position += (mDelta / ((1 * i) + 1));
            }
        }

        mesh.ReDraw();
    }

}