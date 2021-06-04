using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum susBehaviour
{
    casual,
    escaping,
    escaped,
    fighting
}

public class Prisioner : MonoBehaviour
{
    #region BTS
    public Transform exit;
    public Vector3 target;
    public Vector2 size;
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;

    #endregion BTS

    Animator animator;
    #region Stats

    public bool markedFighting;
    public Vector3 fightingPos;
    public susBehaviour currentBehaviour;

    public float EscapeChance = 1;
    public float fightChance = 1;

    public float fightCoolDown = 5;

    public Vector3 escapePos;

    public fight fightImIn;

    #endregion Stats

    public PrisionManager prisionManagerScript;
    void Start()
    {
       animator = GetComponent<Animator>();
        navMeshPath = new NavMeshPath();
        Vector3 pos;
        pos.x = Random.Range(-size.x / 2, size.x / 2);
        pos.z = Random.Range(-size.y / 2, size.y / 2);
        pos.y = 0.5f;
        transform.position = pos;

        GetRandomTarget();

        agent.CalculatePath(target, navMeshPath);
    }

    // Update is called once per frame
    void Update()
    {
        fightCoolDown -= Time.deltaTime;
        float blendValue = Vector3.Magnitude(agent.velocity) / agent.speed;
        switch (currentBehaviour)
        {
            case susBehaviour.casual:
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
                   
                    //tries to escape
                    float randomNumber = Random.Range(0, 100);
                   // Debug.Log("Finsihed path");
                    if (randomNumber < EscapeChance)
                    {
                       // Debug.Log("trying to escape");
                        currentBehaviour = susBehaviour.escaping;
                        agent.CalculatePath(escapePos, navMeshPath);
                    }
                    else
                    {
                        GetRandomTarget();
                    }

                }
                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 255, 255));
                break;
            case susBehaviour.escaping:
                
                agent.SetDestination(escapePos);

                if(Vector3.Distance(transform.position, escapePos) < 3)
                {
                    Debug.Log("Escaped");
                    prisionManagerScript.currentPrisionDamage += prisionManagerScript.escapedPrisonerDamage;
                    currentBehaviour = susBehaviour.escaped;
                }
                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 50, 0));
                break;
            case susBehaviour.fighting:
                agent.CalculatePath(fightingPos, navMeshPath);
                agent.SetDestination(fightingPos);

                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 150, 255));
              
                break;
            case susBehaviour.escaped:
                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 255, 0));
                break;
        }

        animator.SetFloat("Blend", blendValue);
    }

     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Prisioner")
        {
            if ((fightCoolDown <= 0)&& other.transform.GetComponent<Prisioner>().fightCoolDown<=0 && currentBehaviour!=susBehaviour.escaped && other.transform.GetComponent<Prisioner>().currentBehaviour !=susBehaviour.escaped)
            {
               //Debug.Log("hit");

                float randomNumber = Random.Range(0, 100);

                if (randomNumber < fightChance)
                {
                    fightCoolDown = 5;
                    other.transform.GetComponent<Prisioner>().fightCoolDown = 5;
                    prisionManagerScript.attemptToStartAFight(this, other.transform.GetComponent<Prisioner>());
                }
            }
        }
    }

    public void GetRandomTarget()
    {
        target.x = Random.Range(-size.x / 2, size.x / 2);
        target.z = Random.Range(-size.y / 2, size.y / 2);
        target.y = 0.5f;
    }

    public void StopSussyBehaviour()
    {
        if(currentBehaviour == susBehaviour.escaped)
        {

        }
        else if(currentBehaviour == susBehaviour.fighting)
        {
            fightImIn.fighterB.GetRandomTarget();
            fightImIn.fighterA.GetRandomTarget();
            fightImIn.fighterB.currentBehaviour = susBehaviour.casual;
            fightImIn.fighterA.currentBehaviour = susBehaviour.casual;
            prisionManagerScript.CurrentFights.Remove(fightImIn);
        }
        else
        {
            currentBehaviour = susBehaviour.casual;
            GetRandomTarget();
        }

    }
}
