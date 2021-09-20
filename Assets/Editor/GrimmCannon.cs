using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class GrimmCannon : EditorWindow
{
    [MenuItem("Tools/Grimm Cannon")]
    public static void OpenGrimm() => GetWindow<GrimmCannon>();

    public float radius = 2f;
    public int spawnCount = 8;

    SerializedObject so;
    SerializedProperty propRadius;
    SerializedProperty propSpawnCount;

    Vector2[] randPoints;

    void OnEnable()
    {
        so = new SerializedObject(this);
        propRadius = so.FindProperty("radius");
        propSpawnCount = so.FindProperty("spawnCount");

        GenerateRandomPoints();

        SceneView.duringSceneGui += DuringSceneGUI;
    }

    void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    void GenerateRandomPoints()
    {
        randPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randPoints[i] = Random.insideUnitCircle;
        }
    }

    void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propRadius);
        propRadius.floatValue = propRadius.floatValue.AtLeast(0.01f);
        EditorGUILayout.PropertyField(propSpawnCount);
        propSpawnCount.intValue = propSpawnCount.intValue.AtLeast(1);

        if (so.ApplyModifiedProperties())
        {
            GenerateRandomPoints();
            SceneView.RepaintAll();
        }

        // if you clocked left mouse button, in the editor window
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GUI.FocusControl(null);
            Repaint();
        }
    }

    void DrawSphere(Vector3 pos)
    {
        Handles.SphereHandleCap(-1, pos, Quaternion.identity, 0.1f, EventType.Repaint);
    }


    void DuringSceneGUI(SceneView sceneView)
    {
        Handles.zTest = CompareFunction.LessEqual;

        Transform camTf = sceneView.camera.transform;

        if (Event.current.type == EventType.MouseMove)
        {
            sceneView.Repaint();
        }

        // change radius
        if (Event.current.type == EventType.ScrollWheel)
        {
            float scrollDir = Mathf.Sign(Event.current.delta.y);

            so.Update();
            propRadius.floatValue += scrollDir * 0.1f; // scroll speed
            so.ApplyModifiedProperties();
            Repaint();
            Event.current.Use(); // consume the event, don't let it fall through
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
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

            // drawing the points
            foreach (Vector2 p in randPoints)
            {
                // create ray for this point
                Ray ptRay = GetTangentRay(p);

                // raycast to find point on surface
                if (Physics.Raycast(ptRay, out RaycastHit ptHit))
                {
                    // drawi sphere and normal on surface
                    DrawSphere(ptHit.point);
                    Handles.DrawAAPolyLine(ptHit.point, ptHit.point + ptHit.normal);
                }
            }



            // draw circle
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(6, hit.point, hit.point + hitTangent);
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(6, hit.point, hit.point + hitBitangent);
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine(6, hit.point, hit.point + hitNormal);

            // draw circle adapted to terrain
            const int circleDetail = 64;
            Vector3[] ringPoints = new Vector3[circleDetail];
            for (int i = 0; i < circleDetail; i++)
            {
                float t = i / (float)circleDetail - 1; // go back to 0/1 position
                const float TAU = 6.28318530718f;
                float angRad = t * TAU;
                Vector2 dir = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
                Ray r = GetTangentRay(dir);
                if (Physics.Raycast(r,out RaycastHit cHit))
                {
                    ringPoints[i] = cHit.point + cHit.normal * 0.02f;
                }
                else
                {
                    ringPoints[i] = r.origin;
                }
            }
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(ringPoints);

            //Handles.DrawWireDisc(hit.point, hit.normal, radius);
        }
    }

}
