using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 movement;
    float moveHorizontal;
    float moveVertical;
    

    [Header("Movement Speed")]
    float moveSpeed;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f; 

    [Header("Jumping")]
    public float jumpForce = 7.5f;
    public float gravity = 20f;
    public float crouchHeight = 0.5f;

    [Header("Dashing")]
    public float maxDashForce;
    public float dashForce = 1f;
    public float dashDuration = 1f;

    public bool isGrounded = true;
    private bool isDashing = false;
    private bool isCrouching = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveSpeed = walkSpeed;
        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;
        moveVertical = Input.GetAxis("Vertical") * moveSpeed;


        movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement = transform.TransformDirection(movement);


        {
               //Jump Input
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
                }
        }
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
;
        


        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f);

        // Dash Input
        if (Input.GetKey(KeyCode.C) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // Crouch Input
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                transform.localScale = new Vector3(1, crouchHeight, 1);
                isCrouching = true;
            }
            else
            {
                transform.localScale = Vector3.one;
                isCrouching = false;
            }

        }
    }

    void FixedUpdate()
    {
     rb.MovePosition(rb.position + movement * Time.deltaTime);
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            // Interpolate force and use time
            float currentDashForce = Mathf.Lerp(dashForce, maxDashForce, elapsedTime / dashDuration);

            // Apply dash force
            rb.AddForce(transform.forward * currentDashForce * Time.deltaTime, ForceMode.VelocityChange);

            // Elapsed time in realtime
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Reset dash bool
        isDashing = false;
    }
}