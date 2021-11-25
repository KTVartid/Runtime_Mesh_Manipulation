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

    Dictionary<Vector3, DragObject> points = new Dictionary<Vector3, DragObject>();
    List<DragObject> DragObjects = new List<DragObject>();

    int count = 0;




    void Start()
    {
        InitMesh();

        GameObject actualEP;
        Vector3 actualEPpos;
    }

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
        gameObject.AddComponent<isSelected>();
        gameObject.AddComponent<Activator>();


        vertices = clonedMesh.vertices; //4
        triangles = clonedMesh.triangles;
        isCloned = true; //5

        Vector3 dotPos;


        for (int i = 0; i < triangles.Length; i += 3) // végig megyünk az összes háromszögön
        {
            for (int j = 0; j < 3; j++) // végig megyünk a 3 szög adott pontján
            {
                Vector3 currentPoint = vertices[triangles[i + j]];
                DragObject drg;
                if (points.ContainsKey(currentPoint)) //Ha az adott pont már létezik akkor csak kiválasztjuk
                {
                    drg = points[currentPoint];
                }
                else // ha nem létezik akkor létrehozzuk hozzá adjuk az összes listájához a points dict hez és kiválasztjuk
                {
                    GameObject vert = createEP(triangles[i + j]);
                    drg = vert.GetComponent<DragObject>();
                    points[currentPoint] = drg;

                    DragObjects.Add(drg);
                }
                drg.pairedVertices.Add(triangles[i + j]); // a kiválasztott elemhez hozzá adjuk azt a pontot amiért felel
                drg.triangles.Add(i); // a kiválasztott elemhez hozzá adjuk azt a háromszöget amiben szerepel
            }
        }

        for (int i = 0; i < DragObjects.Count; i++) // végig megyünk az összes ponton
        {
            for (int j = i + 1; j < DragObjects.Count; j++) // végig megyünk az összes ponton az adott pont tól
            {
                if (isCommonTriangle(DragObjects[i], DragObjects[j])) // ha van 1 olyan háromszög amiben mind 2 en szerepelnek akkor ök testvérek
                {
                    DragObjects[i].connectedEP.Add(DragObjects[j].gameObject); // hozzá adjuk a testvért
                    DragObjects[j].connectedEP.Add(DragObjects[i].gameObject); // szint ugy csak forditva
                }
            }
        }
    }

    bool isCommonTriangle(DragObject a, DragObject b) // kiválasztja hogy van e közös háromszögük
    {
        for (int i = 0; i < a.triangles.Count; i++)
        {
            for (int j = 0; j < b.triangles.Count; j++)
            {
                if (a.triangles[i] == b.triangles[j])
                {
                    return true;
                }

            }
        }
        return false;
    }

    GameObject createEP(int point)
    {
        GameObject vert = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Material vertMat = Resources.Load("vert_Mat", typeof(Material)) as Material;
        Renderer rend = vert.GetComponent<MeshRenderer>();
        vert.transform.name = "v" + (count);
        count++;
        vert.transform.tag = "EP";
        vert.layer = 9;
        vert.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        vert.transform.parent = gameObject.transform;
        vert.transform.localPosition = vertices[point];
        rend.material.color = Color.blue;
        vert.AddComponent<DragObject>();
        vert.GetComponent<DragObject>().Init(point, this);
        EPList.Add(vert);
        vert.SetActive(false);
        return vert;
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

    public void turnOnEP()
    {
        for (int i = 0; i < EPList.Count; i++)
        {
            EPList[i].SetActive(true);
        }
    }

    public void turnOffEP()
    {
        for (int i = 0; i < EPList.Count; i++)
        {
            EPList[i].SetActive(false);
        }
    }
}