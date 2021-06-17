using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum featureLevel
{
    // when used to tell when

}

public class fight : MonoBehaviour
{
    public Prisioner fighterA;
    public Prisioner fighterB;

    public float currentLength =0;
}

public class PrisionManager : MonoBehaviour
{
    // enum features added

    // list of when each feature gets added

    public List<Prisioner> AllPrisioner;
    
    public List<Transform> wayPoints;

    public List<fight> CurrentFights;

    public float maxPrisionDamage=5;

    public float currentPrisionDamage;

    public GameObject PhysicalPrisionerList;
    public GameObject wayPointObject;

    public float maxFightLength;
    public float passiveFightDamage = 1;
    public float endOfFightDamage = 5;
    public float escapedPrisonerDamage;

    public float currentGameTime =0;
    public float maxGameTime =60;

    public GameManagerScript GameManager;

    #region UI
    public GameObject playerHealthBar;
    public GameObject TimeBar;
    public GameObject candle;
    public GameObject fireParticle;
    public Vector3 originalFirePos;
    #endregion UI


    [Range(0, 1)]
    public float engageSoundVolume;
    [Range(0, 1)]
    public float disengageSoundVolume;

    void Start()
    {
        setUpPrisioners();

        SetUpWayPoints();

        originalFirePos = fireParticle.transform.localPosition;
    }

    void setUpPrisioners()
    {
        for (int i = 0; i < PhysicalPrisionerList.transform.childCount; i++)
        {
            AllPrisioner.Add(PhysicalPrisionerList.transform.GetChild(i).GetComponent<Prisioner>());
            NavMeshAgent agent = PhysicalPrisionerList.transform.GetChild(i).GetComponent<NavMeshAgent>();
            agent.avoidancePriority = i;
        }
    }

    void SetUpWayPoints()
    {
        for (int i = 0; i < wayPointObject.transform.childCount; i++)
        {
            wayPoints.Add(wayPointObject.transform.GetChild(i).transform);

        }
    }

    public Transform GetRandomWayPoint()
    {
        int index = Random.Range(0, wayPoints.Count);
        return wayPoints[index];
    }

    void UpdateUI()
    {
       // Debug.Log((currentPrisionDamage / maxPrisionDamage));
        playerHealthBar.GetComponent<RectTransform>().localScale = new Vector3((currentPrisionDamage / maxPrisionDamage) , playerHealthBar.GetComponent<RectTransform>().localScale.y, playerHealthBar.GetComponent<RectTransform>().localScale.z);
        TimeBar.GetComponent<RectTransform>().localScale = new Vector3((currentGameTime / maxGameTime), TimeBar.GetComponent<RectTransform>().localScale.y, TimeBar.GetComponent<RectTransform>().localScale.z);
    }

    public void attemptToStartAFight(Prisioner A, Prisioner B)
    {
        // check if they're fighting

        if (A.currentBehaviour != susBehaviour.fighting && B.currentBehaviour != susBehaviour.fighting &&
            A.currentBehaviour != susBehaviour.escaping && B.currentBehaviour != susBehaviour.escaping)
        {
            CurrentFights.Add(new fight());
            CurrentFights[CurrentFights.Count - 1].fighterA = A;
            CurrentFights[CurrentFights.Count - 1].fighterB = B;
            A.currentBehaviour = susBehaviour.fighting;
            B.currentBehaviour = susBehaviour.fighting;

            Vector3 middleOfFight = new Vector3((A.transform.position.x + B.transform.position.x) / 2, (A.transform.position.y + B.transform.position.y) / 2, (A.transform.position.z + B.transform.position.z) / 2);

            A.fightingPos = middleOfFight;
            B.fightingPos = middleOfFight;
            A.fightImIn = CurrentFights[CurrentFights.Count - 1];
            B.fightImIn = CurrentFights[CurrentFights.Count - 1];

            A.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 255));
            B.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 255));


            SoundManager.current.PlaySound(Sound.EngageFight, middleOfFight, engageSoundVolume);
            //Debug.Log(middleOfFight);
        }
    }

    void Update()
    {
        float percent = (maxGameTime - currentGameTime) / maxGameTime;

        candle.transform.localScale = new Vector3(1, percent, 1);
        fireParticle.transform.localPosition = new Vector3(fireParticle.transform.localPosition.x, originalFirePos.y - 0.611f * (currentGameTime / maxGameTime), fireParticle.transform.localPosition.z);

        /*
        int index = (int)(candles.Length * percent);
        for (int i = 0; i < candles.Length; i++)
        {
            if (i == index)
            {
                candles[i].SetActive(true);
            }
            else
            {
                candles[i].SetActive(false);
            }
        }
        */

        

        currentGameTime += Time.deltaTime;
        for (int i=0; i<CurrentFights.Count; i++)
        {
            CurrentFights[i].currentLength += Time.deltaTime;
            currentPrisionDamage += Time.deltaTime * passiveFightDamage;

            if (CurrentFights[i].currentLength>=maxFightLength)
            {
                // fight ended.
                currentPrisionDamage += endOfFightDamage;
                CurrentFights[i].fighterA.currentBehaviour = susBehaviour.casual;
                CurrentFights[i].fighterB.currentBehaviour = susBehaviour.casual;
                CurrentFights.RemoveAt(i);


                Prisioner a = CurrentFights[i].fighterA;
                Prisioner b = CurrentFights[i].fighterB;
                Vector3 middleOfFight = new Vector3((a.transform.position.x + b.transform.position.x) / 2, (a.transform.position.y + b.transform.position.y) / 2, (a.transform.position.z + b.transform.position.z) / 2);

                SoundManager.current.PlaySound(Sound.DisengageFight, middleOfFight, disengageSoundVolume);
            }

        }

        if(currentPrisionDamage>=maxPrisionDamage)
        {
            // death
            GameManager.loadLoseScreen();
        }

        if(currentGameTime>= maxGameTime)
        {
            // time out
            GameManager.loadWinScreen();
        }
        UpdateUI();

    }
}
