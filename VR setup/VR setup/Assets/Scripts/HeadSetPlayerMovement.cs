using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeadSetPlayerMovement : MonoBehaviour
{
    public float MoveSpeed;

    #region Public Variables
    public GameObject head;
    public float moveSpeed = 50;
    public float cameraRotateSpeed = 50;
    public Color highlightCol;
    public Color passiveCol;
    public LayerMask mask;
    #endregion
    public LineRenderer lineRenderer;

    #region Private Variables
    float xMoveAmount;
    float yMoveAmount;
    float xTiltAmount;
    float yTiltAmount;
    GameObject lastTouched;
   public bool mounted = false;
    bool unmounting = false;
    public Vector3 positionBefore;

    CharacterController characterController;
    #endregion

    public GameObject handObject;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        passiveLook();

        if (OVRInput.Get(OVRInput.Button.One))
        {
            yMoveAmount = 1;
          
        }
        else
        {
            yMoveAmount = 0;

        }

        Vector3 camForward = handObject.transform.forward; // Used to have the player move based on the camera direction
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = handObject.transform.right;  // use our camera's right vector, which is always horizontal
        camRight.Normalize();

        Vector3 delta = (xMoveAmount * camRight + yMoveAmount * camForward) * moveSpeed * Time.fixedDeltaTime;

        if (!mounted)
        {
            characterController.SimpleMove(delta);
        }

    }

    void FixedUpdate()
    {
     //  Vector3 camForward = head.transform.forward; // Used to have the player move based on the camera direction
     //  camForward.y = 0;
     //  camForward.Normalize();
     // 
     //  Vector3 camRight = head.transform.right;  // use our camera's right vector, which is always horizontal
     //  camRight.Normalize();
     // 
     //  Vector3 delta = (xMoveAmount * camRight + yMoveAmount * camForward) * moveSpeed * Time.fixedDeltaTime;
     // 
     //  if (!mounted)
       {
           //characterController.SimpleMove(delta);
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
        Ray ray = new Ray(handObject.transform.position, handObject.transform.forward);
        RaycastHit[] hits;


        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            lineRenderer.SetColors(new Color(0, 0, 0), new Color(255, 255, 255));
        }
        else
        {
            lineRenderer.SetColors(new Color(0, 255, 0), new Color(0, 255, 0));
        }


            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

        hits = Physics.RaycastAll(ray);
        bool hitted = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (mask == (mask | 1 << hit.collider.gameObject.layer))
            { if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", highlightCol);
                lastTouched = hit.collider.gameObject;
                hitted = true;
               
                    Debug.Log("Hit");
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

                break;

            }
        }
        if (!hitted && lastTouched != null)
        {
            lastTouched.GetComponent<Renderer>().material.SetColor("_Color", passiveCol);
            lastTouched = null;
        }
    }
}

