using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator anim;
    float hinput = 0;
    float jumpTimer = 0;
    float groundTimer = 0;
    float falltimer = 0;
    float noInputTimer = 0;
    bool setFallTimer = false;
    float realHorizontalSpeed = 0;
    bool isJumping = false;
    Vector2 velocity;

   [Header("Variables")]
    public float acceleration = 1;
    public float MaxSpeed = 10.0f;
    public float fallTime = 1.0f;
    public float timeToJump = 0.25f;
    public float timeAfterNoGround = 0.25f;
    public float noInputTime = 0.2f;
    public float inAirDamping = 0.05f;
    public float stopdamping = 0.5f;
    public float turndamping = 0.9f;
    [Header("Jump")]
    public float jumpVelocity = 10;
    public float jumpCutMultiplyer = 0.5f;

    //Ground and Wall checks
    [Header("Ground and Wall checks")]
    public Vector2 groundCheckPosition;
    public Vector2 groundCheckSize;
    public Vector2 leftCheckPosition;
    public Vector2 leftCheckSize;
    public Vector2 rightCheckPosition;
    public Vector2 rightCheckSize;

    public bool onGround = false;
    public bool atRightWall = false;
    public bool atLeftWall = false;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpTimer -= Time.deltaTime;
        groundTimer -= Time.deltaTime;
        falltimer -= Time.deltaTime;
        noInputTimer -= Time.deltaTime;
        //check Wall / ground
        onGround = false;
        atLeftWall = false;
        atRightWall = false;
        if (Physics2D.OverlapBox((Vector2)transform.position + groundCheckPosition, groundCheckSize, 0))
        {
            onGround = true;
            groundTimer = timeAfterNoGround;
            setFallTimer = true;
        }
        if (Physics2D.OverlapBox((Vector2)transform.position + leftCheckPosition, leftCheckSize, 0))
        {
            atLeftWall = true;
            if (setFallTimer == true && rb2d.velocity.y < 0)
            {
                
                falltimer = fallTime;
                setFallTimer = false;
            }
        }
        if (Physics2D.OverlapBox((Vector2)transform.position + rightCheckPosition, rightCheckSize, 0))
        {
            atRightWall = true;
            if (setFallTimer == true && rb2d.velocity.y < 0)
            {
                
                falltimer = fallTime;
                setFallTimer = false;
            }
        }

        //Get input 
        if (noInputTimer < 0)
        {
            hinput = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump"))
            {
                jumpTimer = timeToJump;
            }
            if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0)
            {

                velocity = rb2d.velocity;
                velocity.y *= jumpCutMultiplyer;
                rb2d.velocity = velocity;

            }
        }

        //set direction
        if (hinput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (hinput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

       
    }
    private void FixedUpdate()
    {
        
        if (jumpTimer >0 && groundTimer > 0)
        {
            jumpTimer = 0;
            groundTimer = 0;
            velocity.y = jumpVelocity;
            velocity.x = rb2d.velocity.x;
            rb2d.velocity = velocity;
        }
        if(falltimer > 0 && (atRightWall || atLeftWall))
        {
            Debug.Log("here");
            if (rb2d.velocity.y <= 0)
            {
                Debug.Log("stick");
                velocity.y = 0;
                velocity.x = rb2d.velocity.x;
                rb2d.velocity = velocity;
                rb2d.gravityScale = 0;

                if (jumpTimer > 0)
                {
                    jumpTimer = 0;
                    //falltimer = 0;
                    velocity.y = jumpVelocity;
                    if (atLeftWall)
                    {
                        hinput = 1;
                    }
                    else
                    {
                        hinput = -1;
                    }
                    noInputTimer = noInputTime;
                    velocity.x += MaxSpeed * hinput;
                    rb2d.velocity = velocity;
                }
            }
            else
            {
                rb2d.gravityScale = 1;
                //falltimer = 0;
            }
        }
        else
        {
            rb2d.gravityScale = 1;
            //falltimer = 0;
        }
        if(rb2d.velocity.y < -0.5f)
        {
            rb2d.gravityScale = 2;
        }

        velocity.x = rb2d.velocity.x;
        if (onGround)
        {
            velocity.x += hinput * acceleration;
        }
        else
        {
            velocity.x += hinput * (acceleration/3);
        }

        if (hinput * hinput < 0.01f)
        {
            velocity.x = Mathf.Lerp(0, velocity.x, stopdamping);
        }
        else if (Mathf.Sign(velocity.x) != Mathf.Sign(hinput))
        {
            velocity.x = Mathf.Lerp(0, velocity.x, turndamping);
        }
        else
        { 
            velocity.x = Mathf.Clamp(velocity.x,-MaxSpeed, MaxSpeed);

        }


        
        if(rb2d.velocity.y > 0.1f)
        {
            anim.SetBool("JumpUp", true);
            anim.SetBool("Falling", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Idel", false);
        }

        else if (rb2d.velocity.y < -0.1f)
        {
            anim.SetBool("Falling", true);
            anim.SetBool("JumpUp", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Idel", false);

        }
        else
        {
            if (rb2d.velocity.x * rb2d.velocity.x > 0.1f)
            {
                anim.SetBool("Idel", false);
                anim.SetBool("Walking", true);
                anim.SetBool("Falling", false);
                anim.SetBool("JumpUp", false);
            }
            else
            {
                anim.SetBool("Idel", true);
                anim.SetBool("Walking", false);
                anim.SetBool("Falling", false);
                anim.SetBool("JumpUp", false);
            }
        }
        if(falltimer > 0 && (atRightWall || atLeftWall))
        {
            anim.SetBool("AtWall", true);
            anim.SetBool("Idel", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Falling", false);
            anim.SetBool("JumpUp", false);
        }
        else
        {
            anim.SetBool("AtWall", false);
        }

        //velocity.x = hinput * speed ;
        velocity.y = rb2d.velocity.y;
        rb2d.velocity = velocity;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckPosition, groundCheckSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightCheckPosition, rightCheckSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftCheckPosition, leftCheckSize);
    }
}


