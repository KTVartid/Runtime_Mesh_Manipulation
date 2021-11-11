using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor;

public class BrushTool : MonoBehaviour
{
    [Range(0.01f, 15f)]
    public float radius = 0.3f;
    private int zero = 0;

    Material invisible;

    GameObject cam;
    Camera camera;
    LineRenderer line;

    public List<GameObject> IObjects = new List<GameObject>();

    public List<GameObject> EPoints = new List<GameObject>();
    public List<GameObject> EPrad = new List<GameObject>();
    public GameObject EP;
    public float EPx;
    public float EPy;
    public float EPz;
    public float Cx;
    public float Cy;
    public float Cz;
    public RaycastHit EPHit;
    public RaycastHit hitData;
    public Vector3 EPcoll = Vector3.zero;
    public LayerMask layer;
    public GameObject lastHit;
    public Vector3 rayDir;

    public GameObject mouseSphere;

    GameObject m;
    public float scale;

    private MeshStudy mesh;
    private int Index;

    public float mZCoord;
    public Vector3 mOld;
    public Vector3 mNow;

    SelectTool selectTool;

    void Start()
    {
        mouseSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mouseSphere.transform.name = "mouseSphere";
        mouseSphere.transform.position = new Vector3(999, 999, 999);
        Renderer rend = mouseSphere.GetComponent<MeshRenderer>();
        Material mouseMat = Resources.Load("mouseMat", typeof(Material)) as Material;
        rend.material = mouseMat;
        mouseSphere.layer = 2;

        invisible = Resources.Load("invisible", typeof(Material)) as Material;

        cam = GameObject.Find("Main Camera");
        camera = cam.GetComponent<Camera>();
        line = gameObject.GetComponent<LineRenderer>();
        line.loop = true;
        selectTool = GameObject.Find("Main Camera").GetComponent<SelectTool>();
    }


    void Update()
    {
        scale = GameObject.Find("Slider").GetComponent<Slider>().value;
        if (Input.GetMouseButtonUp(0))
        {
            GameObject.Find("Slider").GetComponent<Slider>().value = 1;
        }
        m = GameObject.FindGameObjectWithTag("Active");
        if (m != null)
        {
            m.transform.localScale = m.transform.localScale * scale;
        }

        createEP();

        Transform camTf = cam.transform;
        // change radius
        float scrollDir = Input.mouseScrollDelta.y;

        radius += scrollDir * 0.02f / 2; // scroll speed

        if (radius < 0)
        {
            radius = 0;
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // setting up tangent space
            Vector3 hitNormal = hit.normal;
            Vector3 hitTangent = Vector3.Cross(hitNormal, camTf.up).normalized;
            Vector3 hitBitangent = Vector3.Cross(hitNormal, hitTangent);

            EPHit = hit;

            Ray GetTangentRay(Vector2 tangentSpacePos)
            {
                Vector3 rayOrigin = hit.point + (hitTangent * tangentSpacePos.x + hitBitangent * tangentSpacePos.y) * radius;
                rayOrigin += hitNormal * 2; // offset margin thing
                Vector3 rayDirection = -hitNormal;
                return new Ray(rayOrigin, rayDirection);
            }

            // draw circle adapted to terrain
            const int circleDetail = 64;
            line.positionCount = circleDetail;
            Vector3[] ringPoints = new Vector3[circleDetail];
            for (int i = 0; i < circleDetail; i++)
            {
                float t = i / (float)circleDetail + 1; // go back to 0/1 position
                const float TAU = 6.28318530718f;
                float angRad = t * TAU;
                Vector2 dir = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
                Ray r = GetTangentRay(dir);
                if (Physics.Raycast(r, out RaycastHit cHit))
                {
                    ringPoints[i] = cHit.point + cHit.normal * 0.02f;
                }
                else
                {
                    ringPoints[i] = r.origin;
                }
                line.SetPosition(i, ringPoints[i]);
                line.SetWidth(0.01f, 0.005f);
                line.material = invisible;
            }
        }

        mouseSphere.transform.localPosition = EPHit.point;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            mouseSphere.transform.localScale = new Vector3(1, 1, 1) * (radius * 2);
        }
        else
        {
            mouseSphere.transform.localScale = new Vector3(1, 1, 1) * 0;
        }

        //check the points inside the sphere

        if (Input.GetMouseButtonDown(0))
        {
            PointsInRadiusCheck();
            mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && EPrad.Count != 0)
        {
            mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mDelta = mNow - mOld;
            mOld = mNow;

            for (int i = 0; i < EPrad.Count; i++)
            {
                mZCoord = Camera.main.WorldToScreenPoint(EPrad[i].transform.position).z;

                EPrad[i].transform.position += mDelta;
                Index = EPrad[i].GetComponent<DragObject>().Index;
                mesh.DoAction(Index, EPrad[i].transform.localPosition, false);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            EPrad.Clear();
        }

        EPColoring();

        EPoints.Clear();
    }

    private void createEP()
    {
        if (m != null)
        {
            mesh = m.GetComponent<MeshStudy>();
        }

        foreach (GameObject points in GameObject.FindGameObjectsWithTag("EP"))
        {
            EPoints.Add(points);
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

    public void PointsInRadiusCheck()
    {
        for (int i = 0; i < EPoints.Count; i++)
        {
            EP = EPoints[i];
            EPx = EP.transform.position.x;
            EPy = EP.transform.position.y;
            EPz = EP.transform.position.z;
            Cx = EPHit.point.x;
            Cy = EPHit.point.y;
            Cz = EPHit.point.z;
            rayDir = (EPoints[i].transform.position - camera.transform.position).normalized;
            Ray camRay = new Ray(camera.transform.position, rayDir);
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (((EPx - Cx) * (EPx - Cx)) + ((EPy - Cy) * (EPy - Cy)) + ((EPz - Cz) * (EPz - Cz)) <= radius * radius && Physics.Raycast(camRay, out hitData) && hitData.collider.tag == "EP")
                {
                    EPrad.Add(EPoints[i]);
                }
            }

        }
    }

    public void EPColoring()
    {
        for (int i = 0; i < EPoints.Count; i++)
        {
            EP = EPoints[i];
            EPx = EP.transform.position.x;
            EPy = EP.transform.position.y;
            EPz = EP.transform.position.z;
            Cx = EPHit.point.x;
            Cy = EPHit.point.y;
            Cz = EPHit.point.z;
            Renderer rend = EPoints[i].GetComponent<MeshRenderer>();
            rayDir = (EPoints[i].transform.position - camera.transform.position).normalized;
            Ray camRay = new Ray(camera.transform.position, rayDir);

            if (Input.GetKey(KeyCode.LeftAlt))
            {

                if (((EPx - Cx) * (EPx - Cx)) + ((EPy - Cy) * (EPy - Cy)) + ((EPz - Cz) * (EPz - Cz)) <= radius * radius && Physics.Raycast(camRay, out hitData) && hitData.collider.tag == "EP")
                {
                    Debug.DrawLine(camera.transform.position, hitData.collider.transform.position, Color.green);
                    rend.material.color = Color.yellow;

                    if (Input.GetMouseButton(0))
                    {
                        rend.material.color = Color.red;
                    }
                }
                else if (rend.material.color != Color.blue)
                {
                    rend.material.color = Color.blue;
                }
            }
            else
            {
                rend.material.color = Color.blue;
            }
        }
    }

}