/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;



public class MeshStudy : MonoBehaviour
{
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;
    public int[] triangles;

    [HideInInspector]
    public Vector3[] vertices;

    [HideInInspector]
    public bool isCloned = false;

    // For Editor
    public float radius = 0.2f;
    public float pull = 0.3f;
    public float handleSize = 0.03f;
    public List<Vector3[]> allTriangleList;
    public bool moveVertexPoint = true;

    public float mZCoord;
    public Vector3 mOld;
    public Vector3 mNow;
    public List<GameObject> EPList = new List<GameObject>();
    public List<GameObject> EPcon;
    public HashSet<GameObject> EPconHash;

    void Start()
    {
        InitMesh();

        GameObject actualEP;
        Vector3 actualEPpos;


        // fill EPs with pairedVertices list
        for (int i = 0; i < EPList.Count; i++)
        {
            actualEP = EPList[i];
            actualEPpos = actualEP.transform.localPosition;
            for (int j = 0; j < vertices.Length; j++)
            {
                if (actualEPpos == vertices[j])
                {
                    List<int> vertIndex = EPList[i].GetComponent<DragObject>().pairedVertices;
                    vertIndex.Add(j);
                }
            }
        }

        // ToDo: egyszerusiteni kell, mert sokaig tart betolteni

        // fill connected EP list
        for (int i = 0; i < EPList.Count; i++)
        {
            GameObject targetEP = EPList[i];
            List<int> targetPairedVertices = targetEP.GetComponent<DragObject>().pairedVertices;

            for (int j = 0; j < targetPairedVertices.Count; j++)
            {
                List<int> connectedVerts = FindConnectedVertices(vertices[targetPairedVertices[j]]);
                for (int k = 0; k < connectedVerts.Count; k++)
                {
                    for (int l = 0; l < EPList.Count; l++)
                    {
                        for (int m = 0; m < EPList[l].GetComponent<DragObject>().pairedVertices.Count; m++)
                        {
                            if (connectedVerts[k] == EPList[l].GetComponent<DragObject>().pairedVertices[m] && !targetEP.GetComponent<DragObject>().connectedEP.Contains(EPList[l]))
                            {
                                targetEP.GetComponent<DragObject>().connectedEP.Add(EPList[l]);
                            }
                        }
                    }
                }
            }
        }
    }

    HashSet<Vector3> dotCoords = new HashSet<Vector3>();

    public void InitMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.sharedMesh; //1
        clonedMesh = new Mesh(); //2

        clonedMesh.name = "clone";
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;
        meshFilter.mesh = clonedMesh;  //3

        vertices = clonedMesh.vertices; //4
        triangles = clonedMesh.triangles;
        isCloned = true; //5

        Vector3 dotPos;

        for (int i = 0; i < vertices.Length; i++)
        {
            dotPos = vertices[i];
            if (dotCoords.Add(dotPos))
            {
                GameObject vert = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Material vertMat = Resources.Load("vert_Mat", typeof(Material)) as Material;
                Renderer rend = vert.GetComponent<MeshRenderer>();
                vert.transform.name = "v" + (0 + i);
                vert.transform.tag = "EP";
                vert.layer = 9;
                vert.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                vert.transform.parent = gameObject.transform;
                vert.transform.localPosition = vertices[i];
                rend.material.color = Color.blue;
                vert.AddComponent<DragObject>();
                vert.GetComponent<DragObject>().Init(i, this);
                EPList.Add(vert);
            }
        }
    }


    public void DoAction(int index, Vector3 localPos, bool isMulti)
    {
        if (isMulti)
        {
            //PullConnectedVertices(index, localPos);
        }

        PullSimilarVertices(index, localPos);

        ReDraw();
    }

    public void ReDraw()
    {
        clonedMesh.vertices = vertices; //4
        clonedMesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = clonedMesh;
    }


    // returns List of int that is related to the targetPt.
    public List<int> FindRelatedVertices(Vector3 targetPt, bool findConnected)
    {
        // list of int
        List<int> relatedVertices = new List<int>();

        int idx = 0;
        Vector3 pos;

        // loop through triangle array of indices
        for (int t = 0; t < triangles.Length; t++)
        {
            // current idx return from tris
            idx = triangles[t];
            // current pos of the vertex
            pos = vertices[idx];
            // if current pos is same as targetPt
            if (pos == targetPt)
            {
                // add to list
                relatedVertices.Add(idx);
                // if find connected vertices
                if (findConnected)
                {
                    // min
                    // - prevent running out of count
                    if (t == 0)
                    {
                        relatedVertices.Add(triangles[t + 1]);
                    }
                    // max 
                    // - prevent runnign out of count
                    if (t == triangles.Length - 1)
                    {
                        relatedVertices.Add(triangles[t - 1]);
                    }
                    // between 1 ~ max-1 
                    // - add idx from triangles before t and after t 
                    if (t > 0 && t < triangles.Length - 1)
                    {
                        relatedVertices.Add(triangles[t - 1]);
                        relatedVertices.Add(triangles[t + 1]);
                    }
                }
            }
        }
        // return compiled list of int
        //Debug.Log(relatedVertices.Count);
        return relatedVertices;
    }

    public void PullSimilarVertices(int index, Vector3 newPos)
    {
        Vector3 targetVertexPos = vertices[index]; //1
        List<int> relatedVertices = FindRelatedVertices(targetVertexPos, false); //2

        foreach (int i in relatedVertices) //3
        {
            vertices[i] = newPos;
        }

    }
    void Update()
    {
        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mOld = mNow;
    }

    public List<int> FindConnectedVertices(Vector3 targetPt)
    {
        // list of int
        List<int> connectedVertices = new List<int>();

        // loop through triangle array of indices
        for (int t = 0; t < triangles.Length; t += 3)
        {
            Vector3 p1 = vertices[triangles[t + 0]];
            Vector3 p2 = vertices[triangles[t + 1]];
            Vector3 p3 = vertices[triangles[t + 2]];

            // if current pos is same as targetPt
            if (targetPt == p1)
            {
                if (!connectedVertices.Contains(triangles[t + 1]))
                {
                    connectedVertices.Add(triangles[t + 1]);
                }
                if (!connectedVertices.Contains(triangles[t + 2]))
                {
                    connectedVertices.Add(triangles[t + 2]);
                }
            }
            if (targetPt == p2)
            {
                if (!connectedVertices.Contains(triangles[t + 0]))
                {
                    connectedVertices.Add(triangles[t + 0]);
                }
                if (!connectedVertices.Contains(triangles[t + 2]))
                {
                    connectedVertices.Add(triangles[t + 2]);
                }
            }
            if (targetPt == p3)
            {
                if (!connectedVertices.Contains(triangles[t + 0]))
                {
                    connectedVertices.Add(triangles[t + 0]);
                }
                if (!connectedVertices.Contains(triangles[t + 1]))
                {
                    connectedVertices.Add(triangles[t + 1]);
                }
            }
        }
        // return compiled list of int
        return connectedVertices;
    }
}