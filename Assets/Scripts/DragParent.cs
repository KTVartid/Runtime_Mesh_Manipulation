using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParent : MonoBehaviour
{
    Vector3 mDelta;
    Vector3 mOld;
    Vector3 mNow;

    string tag;

    private void OnMouseDown()
    {
        mOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tag = this.gameObject.tag;
    }

    private void OnMouseDrag()
    {
        mNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mDelta = mNow - mOld;
        if (tag == "X")
        {
        mDelta = new Vector3(mDelta.x, 0, 0);
        }
        else if (tag == "Y")
        {
            mDelta = new Vector3(0, mDelta.y, 0);
        }
        else if (tag == "Z")
        {
            mDelta = new Vector3(0, 0, mDelta.z);
        }

        mOld = mNow;

        this.transform.parent.position += mDelta;
    }

}
