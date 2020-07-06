using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WASDPlayerController : MonoBehaviour
{

    public float speed = 1;
    Rigidbody rigidbody;
    Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        setRigidboby();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector -= transform.right;
        }
    }

    private void FixedUpdate()
    {
        
        rigidbody.MovePosition(transform.position+moveVector*speed*Time.fixedDeltaTime);
    }

    void setRigidboby()
    {
        rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
