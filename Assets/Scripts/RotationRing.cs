using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRing : MonoBehaviour
{
    //manual settings
    [Range(20, 120)]
    public int segments = 60;
    public float innerRadius = 1.4f;
    public float thickness = 0.05f;
    public Material ringMat;

    Mesh ringMesh;

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
        X.transform.tag = "X";
        X.AddComponent<MeshFilter>();
        X.AddComponent<MeshRenderer>();
        X.transform.parent = transform;
        X.transform.localScale = Vector3.one;
        X.transform.localPosition = Vector3.zero;
        X.transform.localRotation = Quaternion.Euler(0, 0, 90);
        X.GetComponent<Renderer>().material.color = Color.red;
        X.AddComponent<RotateParent>();
        X.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        X.AddComponent<MeshCollider>();
        X.AddComponent<changeColor>();
        X.SetActive(false);

        GameObject Y = new GameObject("YRot");
        Y.transform.tag = "Y";
        Y.AddComponent<MeshFilter>();
        Y.AddComponent<MeshRenderer>();
        Y.transform.parent = transform;
        Y.transform.localScale = Vector3.one;
        Y.transform.localPosition = Vector3.zero;
        Y.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Y.GetComponent<Renderer>().material.color = Color.green;
        Y.AddComponent<RotateParent>();
        Y.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        Y.AddComponent<MeshCollider>();
        Y.AddComponent<changeColor>();
        Y.SetActive(false);

        GameObject Z = new GameObject("ZRot");
        Z.transform.tag = "Z";
        Z.AddComponent<MeshFilter>();
        Z.AddComponent<MeshRenderer>();
        Z.transform.parent = transform;
        Z.transform.localScale = Vector3.one;
        Z.transform.localPosition = Vector3.zero;
        Z.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Z.GetComponent<Renderer>().material.color = Color.blue;
        Z.AddComponent<RotateParent>();
        Z.GetComponent<MeshFilter>().sharedMesh = ringMesh;
        Z.AddComponent<MeshCollider>();
        Z.AddComponent<changeColor>();
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
        X.transform.name = "X";
        X.transform.tag = "X";
        X.transform.parent = gameObject.transform;
        X.transform.localPosition = new Vector3(0.9f, 0f, 0f);
        X.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        X.transform.localRotation = Quaternion.Euler(90, 90, 0);
        X.GetComponent<CapsuleCollider>().direction = 0;
        X.GetComponent<Renderer>().material.color = Color.red;
        X.AddComponent<DragParent>();
        X.AddComponent<changeColor>();
        X.SetActive(false);

        GameObject Y = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Y.transform.name = "Y";
        Y.transform.tag = "Y";
        Y.transform.parent = gameObject.transform;
        Y.transform.localPosition = new Vector3(0f, 1.4f, 0f);
        Y.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        Y.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Y.GetComponent<CapsuleCollider>().direction = 1;
        Y.GetComponent<Renderer>().material.color = Color.green;
        Y.AddComponent<DragParent>();
        Y.AddComponent<changeColor>();
        Y.SetActive(false);

        GameObject Z = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Z.transform.name = "Z";
        Z.transform.tag = "Z";
        Z.transform.parent = gameObject.transform;
        Z.transform.localPosition = new Vector3(0f, 0f, 0.9f);
        Z.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        Z.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Z.GetComponent<CapsuleCollider>().direction = 2;
        Z.GetComponent<Renderer>().material.color = Color.blue;
        Z.AddComponent<DragParent>();
        Z.AddComponent<changeColor>();
        Z.SetActive(false);
    }

}
