using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class BrushTool : MonoBehaviour
{


    public float radius = 0.3f;

    GameObject cam;
    Camera camera;
    LineRenderer line;


    void Start()
    {
        cam = GameObject.Find("Main Camera");
        camera = cam.GetComponent<Camera>();
        line = gameObject.GetComponent<LineRenderer>();
    }


    void Update()
    {
        Transform camTf = cam.transform;
        // change radius
        float scrollDir = Input.mouseScrollDelta.y;
        radius += scrollDir * 0.1f / 2; // scroll speed



        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(camTf.position, camTf.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // setting up tangent space
            Vector3 hitNormal = hit.normal;
            Vector3 hitTangent = Vector3.Cross(hitNormal, camTf.up).normalized;
            Vector3 hitBitangent = Vector3.Cross(hitNormal, hitTangent);

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
                line.SetWidth(0.02f, 0.02f);
            }
        }
    }

}
