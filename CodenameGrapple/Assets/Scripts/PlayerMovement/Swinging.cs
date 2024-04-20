using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Transforms")]
    Rigidbody rb;
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    

    [Header("Swing Settings")]
    public LayerMask WhatIsGrappleable;
    public float maxSwingDistance = 25f;
    public float ropeLengthMax;
    public float ropeLengthMin;
    public float swingBoost;
    Vector3 swingPoint;
    bool isSwinging;

    [Header("Spring Joint Settings")]
    private SpringJoint joint;
    public float jointSpring = 10f;
    public float jointDamper = 3f;
    public float jointMassScale = 4.5f;
    
    
    private Vector3 currentGrapplePosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) StartSwing();
        if (Input.GetKeyUp(KeyCode.Mouse1)) StopSwing();
        
        if (isSwinging) 
        {
            rb.AddRelativeForce(Vector3.forward * swingBoost * Time.deltaTime);
        }
        
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        //Draw Rope and interp movement of line renderer
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void StartSwing()
    {
        //Send raycast from camera position to find position to attach
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, WhatIsGrappleable))
        {
            //Spawn Spring Join Component
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);
            
            //Min and max length of joint
            joint.maxDistance = distanceFromPoint * ropeLengthMax;
            joint.minDistance = distanceFromPoint * ropeLengthMin;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointDamper;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            isSwinging = true;
        }
    }
    void StopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
        isSwinging = false;
    }
}
