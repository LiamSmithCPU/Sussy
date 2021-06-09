using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<fight> CurrentFights;

    public float maxPrisionDamage=5;

    public float currentPrisionDamage;

    public GameObject PhysicalPrisionerList;

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
    #endregion UI

    void Start()
    {
        setUpPrisioners();
    }

    void setUpPrisioners()
    {
        for (int i = 0; i < PhysicalPrisionerList.transform.childCount; i++)
        {
            AllPrisioner.Add(PhysicalPrisionerList.transform.GetChild(i).GetComponent<Prisioner>());
          
        }
            
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

        if (A.currentBehaviour!= susBehaviour.fighting && B.currentBehaviour != susBehaviour.fighting)
        {
            CurrentFights.Add(new fight());
           CurrentFights[CurrentFights.Count - 1].fighterA  = A;
            CurrentFights[CurrentFights.Count - 1].fighterB = B;
            A.currentBehaviour = susBehaviour.fighting;
            B.currentBehaviour = susBehaviour.fighting;

            Vector3 middleOfFight = new Vector3((A.transform.position.x + B.transform.position.x) / 2, (A.transform.position.y + B.transform.position.y) / 2, (A.transform.position.z + B.transform.position.z) / 2);
            A.fightingPos = middleOfFight;
            B.fightingPos = middleOfFight;
            A.fightImIn = CurrentFights[CurrentFights.Count - 1];
            B.fightImIn = CurrentFights[CurrentFights.Count - 1];

            A.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0,0,255));
            B.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 255));

            //Debug.Log(middleOfFight);
        }
    }

    void Update()
    {
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
