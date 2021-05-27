using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardPlayerMovement : MonoBehaviour
{
    public Camera head;

    public float moveSpeed;
    public float cameraRotateSpeed = 2;

    public float xMoveAmount;
    public float yMoveAmount;

    public float xTiltAmount;
    public float yTiltAmount;

    LineRenderer lineRenderer;

    GameObject lastTouched;

    public Color highlightCol;
    public Color passiveCol;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        passiveLook();
        if (Input.GetMouseButtonDown(0))
        {
            PointAtTarget();
        }
        xMoveAmount = Input.GetAxis("Horizontal");
        yMoveAmount = Input.GetAxis("Vertical");

        xTiltAmount = Input.GetAxis("Mouse Y");
        yTiltAmount = Input.GetAxis("Mouse X");


        head.transform.Rotate(new Vector3(-xTiltAmount, yTiltAmount));
    }

    private void FixedUpdate()
    {

        Vector3 camForward = head.transform.forward; // Used to have the player move based on the camera direction
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = head.transform.right;  // use our camera's right vector, which is always horizontal
        camRight.Normalize();


        Vector3 delta = (xMoveAmount * camRight + yMoveAmount * camForward) * moveSpeed;

        this.transform.position = new Vector3(this.transform.position.x + delta.x, this.transform.position.y + delta.y, this.transform.position.z + delta.z);
        // this.transform.position = new Vector3(this.transform.position.x + xMoveAmount * moveSpeed , this.transform.position.y, this.transform.position.z + yMoveAmount * moveSpeed);
        // actually move 
    }

    void PointAtTarget()
    {
        // raycast
        // if pris do something 
        Ray ray = new Ray(head.transform.position, head.transform.forward);
        RaycastHit hit;

        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
            {
                hit.collider.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 0, 0));
            }
                    
            }
    }

    void passiveLook()
    {
        // raycast
        // if pris do something 
        Ray ray = new Ray(head.transform.position, head.transform.forward);
        RaycastHit hit;

        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

        if (Physics.Raycast(ray, out hit))
        {
            Rigidbody body = hit.collider.GetComponent<Rigidbody>();
            if (body)
            {
                hit.collider.GetComponent<Renderer>().material.SetColor("_Color", highlightCol);
                lastTouched = hit.collider.gameObject;
            }
            else
            {
                if(lastTouched != null)
                {
                lastTouched.GetComponent<Renderer>().material.SetColor("_Color", passiveCol);

                }
            }

        }
    }
}
