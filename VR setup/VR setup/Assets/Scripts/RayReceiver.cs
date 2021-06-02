using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayReceiver : MonoBehaviour
{
    protected bool hit = false;

    public void Hit()
    {
        hit = true;
    }

    void LateUpdate()
    {
        hit = false;
    }

}
