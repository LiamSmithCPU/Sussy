using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLines : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject hand;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(hand.transform.position, hand.transform.forward);
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);
    }
}
