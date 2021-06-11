using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalPusher : MonoBehaviour
{
    public Transform pointer;
    LineRenderer lineRenderer;

     void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(pointer.position, pointer.forward);
        RaycastHit hit;

        lineRenderer.SetPosition(0, ray.origin); 
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
                    body.AddForce(100.0f * ray.direction);
            }
        }
    }
}

