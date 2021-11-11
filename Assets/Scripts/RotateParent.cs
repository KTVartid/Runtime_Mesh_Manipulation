using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParent : MonoBehaviour
{
    Vector3 mDelta;
    Vector3 mOld;
    Vector3 mNow;

    private void OnMouseDown()
    {
        tag = this.gameObject.tag;
        mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    void OnMouseDrag()
    {
        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mDelta = mNow - mOld;

        if (tag == "X")
        {
            transform.parent.Rotate(-mDelta.x, 0, 0);
        }
        else if (tag == "Y")
        {
            transform.parent.Rotate(0, -mDelta.x, 0);
        }
        else if (tag == "Z")
        {
            transform.parent.Rotate(0, 0, -mDelta.x);
        }
        mOld = mNow;

    }

    private void OnMouseUp()
    {
    }

}
