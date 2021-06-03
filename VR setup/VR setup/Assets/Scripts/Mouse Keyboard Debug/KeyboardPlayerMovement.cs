using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardPlayerMovement : MonoBehaviour
{
    #region Public Variables
    public Camera head;
    public float moveSpeed = 50;
    public float cameraRotateSpeed = 50;
    public Color highlightCol;
    public Color passiveCol;
    #endregion

    #region Private Variables
    float xMoveAmount;
    float yMoveAmount;
    float xTiltAmount;
    float yTiltAmount;
    LineRenderer lineRenderer;
    GameObject lastTouched;
    #endregion

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

        xTiltAmount += Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;
        yTiltAmount -= Input.GetAxis("Mouse Y") * cameraRotateSpeed * Time.deltaTime;
        yTiltAmount = Mathf.Clamp(yTiltAmount, -90.0f, 90.0f);

        head.transform.localRotation = Quaternion.Euler(yTiltAmount, 0.0f, 0.0f);
        transform.eulerAngles = new Vector3(0.0f, xTiltAmount, 0.0f);

        //head.transform.Rotate(new Vector3(-xTiltAmount, yTiltAmount));
    }

    private void FixedUpdate()
    {

        Vector3 camForward = head.transform.forward; // Used to have the player move based on the camera direction
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = head.transform.right;  // use our camera's right vector, which is always horizontal
        camRight.Normalize();

        Vector3 delta = (xMoveAmount * camRight + yMoveAmount * camForward) * moveSpeed * Time.fixedDeltaTime;

        this.transform.position = new Vector3(this.transform.position.x + delta.x, this.transform.position.y + delta.y, this.transform.position.z + delta.z); // actually move 
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
                hit.collider.gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", highlightCol);
                lastTouched = hit.collider.gameObject;
                Debug.Log("Hit");
            }
            else
            {
                if (lastTouched != null)
                {
                    lastTouched.GetComponent<Renderer>().material.SetColor("_Color", passiveCol);
                }
            }

        }
    }
}
