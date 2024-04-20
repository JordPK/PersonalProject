using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatisWall;
    public PlayerController playerController;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    public bool climbing;

    [Header("Wall Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    bool wallFront;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WallCheck();
        StateMachine();
        if (climbing) ClimbingMovement();
    }

    private void WallCheck()
    {
        //
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatisWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (playerController.isGrounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    void StateMachine()
    {
        // State 1 - Vertical Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            //Timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        //State  - Null
        else
        {
            if (climbing) StopClimbing();
        }
    }

    void StartClimbing()
    {
        climbing = true;
    }

    void ClimbingMovement()
    {
        rb.AddForce(Vector3.up * climbSpeed);
    }

    void HorizontalClimbingMovement()
    {
        rb.velocity = new Vector3(climbSpeed, rb.velocity.y, rb.velocity.z);
    }

    void StopClimbing()
    {
        climbing = false;
    }
}
