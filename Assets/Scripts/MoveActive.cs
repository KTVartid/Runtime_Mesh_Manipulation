using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActive : MonoBehaviour
{
    Vector3 mDelta;
    Vector3 mOld;
    Vector3 mNow;

    GameObject cam;

    GameObject active;
    List<GameObject> axles;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        axles = cam.GetComponent<CreateTransformationGizmos>().axles;
    }

    private void Update()
    {
        active = GameObject.FindGameObjectWithTag("Active");
        if (active != null)
        {
            Vector3 activeSize = cam.GetComponent<SelectTool>().unifiedSize;

            List<float> activeSizeAxis = new List<float>();
            activeSizeAxis.Add(activeSize.x);
            activeSizeAxis.Add(activeSize.y);
            activeSizeAxis.Add(activeSize.z);

            List<float> activePosition = new List<float>();
            activePosition.Add(active.transform.position.x);
            activePosition.Add(active.transform.position.y);
            activePosition.Add(active.transform.position.z);

            axles[0].transform.position = new Vector3(activeSizeAxis[0] + activePosition[0], activePosition[1], activePosition[2]);
            axles[1].transform.position = new Vector3(activePosition[0], activeSizeAxis[1] + activePosition[1], activePosition[2]);
            axles[2].transform.position = new Vector3(activePosition[0], activePosition[1], activeSizeAxis[2] + activePosition[2]);

            for (int i = 0; i < axles.Count; i++)
            {
                axles[i].transform.localScale = new Vector3(active.transform.localScale.x / 50f, active.transform.localScale.y / 5f, active.transform.localScale.z / 50f);
            }
        }
    }

    private void OnMouseDown()
    {
        mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tag = this.gameObject.tag;
    }

    private void OnMouseUp()
    {
        cam.GetComponent<SelectTool>().fetchTargetSize();
    }

    private void OnMouseDrag()
    {
        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mDelta = mNow - mOld;
        if (name == "XMove")
        {
            mDelta = new Vector3(mDelta.x, 0, 0);
        }
        else if (name == "YMove")
        {
            mDelta = new Vector3(0, mDelta.y, 0);
        }
        else if (name == "ZMove")
        {
            mDelta = new Vector3(0, 0, mDelta.z);
        }

        mOld = mNow;

        active.transform.position += mDelta;
    }

}
