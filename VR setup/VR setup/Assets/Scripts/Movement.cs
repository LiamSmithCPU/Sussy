using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform cam;
    public float speed = 3;
    // Update is called once per frame
    void Update()
    {
        if (cam.eulerAngles.x > 10 && cam.eulerAngles.x < 30)
        {
            Vector3 fwd = cam.forward;
            fwd.y = 0;
            transform.position += fwd * speed * Time.deltaTime;
        }
    }
}
