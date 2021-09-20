using UnityEngine;
using System.Collections;

public class BrushScr : MonoBehaviour
{
    public int segments = 10;
    public float xradius = 1;
    public float yradius = 1;

    void Start()
    {
        CreatePoints();
    }

    void Update()
    {

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;



    }



    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;


            angle += (360f / segments);
        }
    }
}
