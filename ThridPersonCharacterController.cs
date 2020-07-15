using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonCharacterController : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public float speed = 5;
    float turnSoomthTime = 0.1f;

    float turnSoomthVelocity;

    private float gravitySpeed = 0;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 movedir = Vector3.zero;
        if (direction.sqrMagnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSoomthVelocity, turnSoomthTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            movedir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }
            movedir = movedir.normalized * speed ;

            if (controller.isGrounded)
            {
                gravitySpeed = 0;
            }
            else
            {
                gravitySpeed -= 98.1f*Time.deltaTime;
                
            }
            movedir.y = gravitySpeed;

            controller.Move(movedir * Time.deltaTime);

            
        
    }
}
