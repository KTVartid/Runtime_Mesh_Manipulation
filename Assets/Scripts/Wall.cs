using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(Camera.main.transform);
        transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

}
