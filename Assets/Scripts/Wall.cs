using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        transform.LookAt(Camera.main.transform);
        transform.localEulerAngles = new Vector3(-90, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
