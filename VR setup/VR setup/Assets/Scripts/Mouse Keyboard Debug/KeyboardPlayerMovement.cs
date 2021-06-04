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
    public LayerMask mask;
    #endregion

    #region Private Variables
    float xMoveAmount;
    float yMoveAmount;
    float xTiltAmount;
    float yTiltAmount;
    LineRenderer lineRenderer;
    GameObject lastTouched;
    bool mounted = false;
    bool unmounting = false;
    public Vector3 positionBefore;

    CharacterController characterController;
    #endregion

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        passiveLook();
        //if (Input.GetMouseButtonDown(0))
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (!mounted)
        //    {
        //        PointAtSpotlight();
        //    }
        //    else
        //    {
        //        mounted = false;
        //        characterController.enabled = false;
        //        transform.localPosition = positionBefore;
        //        characterController.enabled = true;
        //    }
        //}

        xMoveAmount = Input.GetAxis("Horizontal");
        yMoveAmount = Input.GetAxis("Vertical");

        xTiltAmount += Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;
        yTiltAmount -= Input.GetAxis("Mouse Y") * cameraRotateSpeed * Time.deltaTime;
        yTiltAmount = Mathf.Clamp(yTiltAmount, -90.0f, 90.0f);

        head.transform.localRotation = Quaternion.Euler(yTiltAmount, 0.0f, 0.0f);
        transform.eulerAngles = new Vector3(0.0f, xTiltAmount, 0.0f);

        //head.transform.Rotate(new Vector3(-xTiltAmount, yTiltAmount));

    }

    void FixedUpdate()
    {
        Vector3 camForward = head.transform.forward; // Used to have the player move based on the camera direction
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = head.transform.right;  // use our camera's right vector, which is always horizontal
        camRight.Normalize();

        Vector3 delta = (xMoveAmount * camRight + yMoveAmount * camForward) * moveSpeed * Time.fixedDeltaTime;

        if (!mounted)
        {
            characterController.SimpleMove(delta);
        }
    }

    void PointAtSpotlight()
    {
        // raycast
        // if pris do something 
        Ray ray = new Ray(head.transform.position, head.transform.forward);
        RaycastHit hit;

        //lineRenderer.SetPosition(0, ray.origin);
        //lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

        if (Physics.Raycast(ray, out hit))
        {
            WatchTower spotLight = hit.collider.GetComponent<WatchTower>();

            if (spotLight)
            {
                hit.collider.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 0, 0));
                positionBefore = transform.position;
                transform.localPosition = spotLight.mount.localPosition;
                //transform.SetParent(spotLight.mount);
                mounted = true;
            }
        }
    }

    void passiveLook()
    {
        // raycast
        // if pris do something 
        Ray ray = new Ray(head.transform.position, head.transform.forward);
        RaycastHit[] hits;

        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

        hits = Physics.RaycastAll(ray);
        bool hitted = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (mask == (mask | 1 << hit.collider.gameObject.layer))
            {
                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", highlightCol);
                    lastTouched = hit.collider.gameObject;
                    hitted = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Transform parent = hit.collider.gameObject.transform.parent.parent;
                        if (parent)
                        {
                            Prisioner prisioner = parent.GetComponent<Prisioner>();
                            if (prisioner)
                            {
                                prisioner.StopSussyBehaviour();
                            }
                        }
                    }
                    Debug.Log("Hit");
                    break;
                }
            }
        }
        if (!hitted && lastTouched != null)
        {
            lastTouched.GetComponent<Renderer>().material.SetColor("_Color", passiveCol);
            lastTouched = null;
        }
    }
}
