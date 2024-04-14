using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Callbacks;
using UnityEngine;

public class SwingHook : MonoBehaviour
{

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    private float maxSwingDist = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;
    // Start is called before the first frame update

    [Header("AirMovement")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    private Vector3 currentGrapplePosition;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        checkForSwingPoints();

        if (joint != null) AirMovement();
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void checkForSwingPoints(){
        if (joint !=null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDist, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDist, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero){
            realHitPoint = raycastHit.point;
        }
        
        else if(sphereCastHit.point != Vector3.zero){
            realHitPoint = sphereCastHit.point;
        }
        else
        {
            realHitPoint = Vector3.zero;
        }

        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }   

    private void StartSwing()
    {
        if (predictionHit.point == Vector3.zero) return;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distFromPoint = Vector3.Distance(player.position,swingPoint);

        // since we're using spring joint to hack it, this is the range you can bounce from. 
        joint.maxDistance = distFromPoint * 0.8f;
        joint.minDistance = distFromPoint * 0.25f;
        // play with this to get the values we want. 
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;
        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime*8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void AirMovement()
    {
        // left
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        // right
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        // forward
        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        
        // shorten cable

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint-transform.position;

            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distFromPoint = Vector3.Distance(player.position,swingPoint);

            // since we're using spring joint to hack it, this is the range you can bounce from. 
            joint.maxDistance = distFromPoint * 0.8f;
            joint.minDistance = distFromPoint * 0.25f;
        }

        if (Input.GetKey(KeyCode.S)){
            float extendedDistFromPoint = Vector3.Distance(player.position,swingPoint) + extendCableSpeed;

            // since we're using spring joint to hack it, this is the range you can bounce from. 
            joint.maxDistance = extendedDistFromPoint * 0.8f;
            joint.minDistance = extendedDistFromPoint * 0.25f;
        }
    }
}
