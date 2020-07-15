using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDolly : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 10;
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        if(Input.GetMouseButton(0)){ 
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        }
    }
}
