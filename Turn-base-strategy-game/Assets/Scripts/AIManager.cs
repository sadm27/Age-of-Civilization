using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    //public GameObject PlayerAI;
    public Player AiPlayerScript;//adsada
    //public GameObject GC;
    public GameController GCScript;
    //public GameObject MM;
    public MouseManagerS MMS;



    GameObject[] units;
    GameObject unit;
    Unit[] Uscripts;
    Unit Uscript;

    public string CurrPlayerCheck;

    // Use this for initialization
    void Start () {

        AiPlayerScript = PlayerAI.GetComponent<Player>();
        GCScript = GC.GetComponent<GameController>();
        MMS = MM.GetComponent<MouseManagerS>();

    }
	
	// Update is called once per frame
	void Update () {

        CurrPlayerCheck = GC.GetCurrPlayer();

    }


    void AiMoveUnit()
    {

        if(CurrPlayerCheck == AiPlayerScript.tag.ToString())
        {

        }

    }


}
