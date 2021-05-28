using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
enum susBehaviour
{
    peaceful,
    escaping,
    fighting

}

public class Prisioner : MonoBehaviour
{
    public Transform exit;
    public Vector3 target;
    public Vector2 size;
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;
    // Start is called before the first frame update
    void Start()
    {
        navMeshPath = new NavMeshPath();
        Vector3 pos;
        pos.x = Random.Range(-size.x / 2, size.x / 2);
        pos.z = Random.Range(-size.y / 2, size.y / 2);
        pos.y = 0.5f;
        transform.position = pos;

        GetRandomTarget();

    }

    // Update is called once per frame
    void Update()
    {
       
        // NavMeshPath navMeshPath;
        agent.CalculatePath(target, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            GetRandomTarget();
        }
        agent.SetDestination(target);

        if (Vector3.Distance(transform.position, target) < 2)
        {
            // stay for a period of time
            // randomise a idle time
            GetRandomTarget();
        }
    }

    void GetRandomTarget()
    {
        target.x = Random.Range(-size.x / 2, size.x / 2);
        target.z = Random.Range(-size.y / 2, size.y / 2);
        target.y = 0.5f;
    }
}
