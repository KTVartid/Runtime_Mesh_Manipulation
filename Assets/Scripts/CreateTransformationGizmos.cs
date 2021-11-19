using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTransformationGizmos : MonoBehaviour
{
    int segments = 60;
    float innerRadius = 1.4f;
    float thickness = 0.05f;

    Mesh ringMesh;

    public List<GameObject> axles = new List<GameObject>();
    public List<GameObject> rings = new List<GameObject>();


    void Start()
    {
        SetUpRings();
        BuildRingMesh();
        CreateAxles();
    }


    void SetUpRings()
    {
        ringMesh = new Mesh();

        GameObject X = new GameObject("XRot");
        X.transform.tag = "Rotation";
        X.layer = 12;
        X.AddComponent<MeshFilter>();
        X.AddComponent<MeshRenderer>();
        X.transform.localPosition = Vector3.zero;
        X.transform.localRotation = Quaternion.Euler(0, 0, 90);
        X.GetComponent<Renderer>().material.color = Color.red;
        X.AddComponent<RotateActive>();
        X.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        X.AddComponent<MeshCollider>();
        X.AddComponent<changeColor>();
        rings.Add(X);
        X.SetActive(false);

        GameObject Y = new GameObject("YRot");
        Y.transform.tag = "Rotation";
        Y.layer = 12;
        Y.AddComponent<MeshFilter>();
        Y.AddComponent<MeshRenderer>();
        Y.transform.localPosition = Vector3.zero;
        Y.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Y.GetComponent<Renderer>().material.color = Color.green;
        Y.AddComponent<RotateActive>();
        Y.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        Y.AddComponent<MeshCollider>();
        Y.AddComponent<changeColor>();
        rings.Add(Y);
        Y.SetActive(false);

        GameObject Z = new GameObject("ZRot");
        Z.transform.tag = "Rotation";
        Z.layer = 12;
        Z.AddComponent<MeshFilter>();
        Z.AddComponent<MeshRenderer>();
        Z.transform.localPosition = Vector3.zero;
        Z.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Z.GetComponent<Renderer>().material.color = Color.blue;
        Z.AddComponent<RotateActive>();
        Z.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        Z.AddComponent<MeshCollider>();
        Z.AddComponent<changeColor>();
        rings.Add(Z);
        Z.SetActive(false);
    }

    void BuildRingMesh()
    {
        //build ring mesh
        Vector3[] vertices = new Vector3[(segments + 1) * 2 * 2];
        int[] triangles = new int[segments * 6 * 2];
        Vector2[] uv = new Vector2[(segments + 1) * 2 * 2];
        int halfway = (segments + 1) * 2;

        for (int i = 0; i < segments + 1; i++)
        {
            float progress = (float)i / (float)segments;
            float angle = Mathf.Deg2Rad * progress * 360;
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            vertices[i * 2] = vertices[i * 2 + halfway] = new Vector3(x, 0f, z) * (innerRadius + thickness);
            vertices[i * 2 + 1] = vertices[i * 2 + 1 + halfway] = new Vector3(x, 0f, z) * innerRadius;
            uv[i * 2] = uv[i * 2 + halfway] = new Vector2(progress, 0f);
            uv[i * 2 + 1] = uv[i * 2 + 1 + halfway] = new Vector2(progress, 1f);

            if (i != segments)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = (i + 1) * 2;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = i * 2 + 1;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2 + halfway;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = i * 2 + 1 + halfway;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = (i + 1) * 2 + halfway;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1 + halfway;
            }

        }

        if (vertices.Length < ringMesh.vertices.Length)
        {
            ringMesh.triangles = triangles;
            ringMesh.vertices = vertices;
        }
        else
        {
            ringMesh.vertices = vertices;
            ringMesh.triangles = triangles;
        }
        ringMesh.uv = uv;
        ringMesh.RecalculateNormals();
    }

    void CreateAxles()
    {
        GameObject X = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        X.transform.name = "XMove";
        X.transform.tag = "Move";
        X.layer = 12;
        X.transform.localPosition = Vector3.zero;
        X.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        X.transform.Rotate(0, 0, 90, Space.World);
        Destroy(X.GetComponent<CapsuleCollider>());
        X.AddComponent<BoxCollider>();
        X.GetComponent<Renderer>().material.color = Color.red;
        X.AddComponent<MoveActive>();
        X.AddComponent<changeColor>();
        axles.Add(X);
        X.SetActive(false);

        GameObject Y = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Y.transform.name = "YMove";
        Y.transform.tag = "Move";
        Y.layer = 12;
        Y.transform.localPosition = Vector3.zero;
        Y.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        Y.transform.Rotate(0, 90, 0, Space.World);
        Destroy(Y.GetComponent<CapsuleCollider>());
        Y.AddComponent<BoxCollider>();
        Y.GetComponent<Renderer>().material.color = Color.green;
        Y.AddComponent<MoveActive>();
        Y.AddComponent<changeColor>();
        axles.Add(Y);
        Y.SetActive(false);

        GameObject Z = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Z.transform.name = "ZMove";
        Z.transform.tag = "Move";
        Z.layer = 12;
        Z.transform.localPosition = Vector3.zero;
        Z.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        Z.transform.Rotate(90, 0, 0, Space.World);
        Destroy(Z.GetComponent<CapsuleCollider>());
        Z.AddComponent<BoxCollider>();
        Z.GetComponent<Renderer>().material.color = Color.blue;
        Z.AddComponent<MoveActive>();
        Z.AddComponent<changeColor>();
        axles.Add(Z);
        Z.SetActive(false);
    }

    public void turnOnAxles()
    {
        for (int i = 0; i < axles.Count; i++)
        {
            axles[i].SetActive(true);
        }
    }

    public void turnOffAxles()
    {
        for (int i = 0; i < axles.Count; i++)
        {
            axles[i].SetActive(false);
        }
    }

    public void turnOnRings()
    {
        for (int i = 0; i < rings.Count; i++)
        {
            rings[i].SetActive(true);
        }
    }

    public void turnOffRings()
    {
        for (int i = 0; i < rings.Count; i++)
        {
            rings[i].SetActive(false);
        }
    }
}
