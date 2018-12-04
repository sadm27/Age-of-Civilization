using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public Player AiPlayerScript;//adsada
    public GameController GCScript;
    public MouseManagerS MMS;
    public TileMap map;



    public GameObject[] units;
    public GameObject unit;
    //Unit[] Uscripts;
    //Unit Uscript;

    public string CurrPlayerCheck;
    public string AIPlayerStr;

    // Use this for initialization
    void Start () {

        CurrPlayerCheck = GCScript.GetCurrPlayer();
        seltectAIPlayerUnits();


    }
	
	// Update is called once per frame
	void Update () {

        CurrPlayerCheck = GCScript.GetCurrPlayer();
        AiMoveUnit();
    }


    void AiMoveUnit()
    {
        Debug.Log("CurrPlayer: " + CurrPlayerCheck);
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString())
        {

            foreach (GameObject unit in units)
            {
                MMS.SelectUnit(unit);
                Unit Uscript1 = unit.GetComponent<Unit>();
                int x = Uscript1.Xtile;
                int y = Uscript1.Xtile;

                map.MoveSelectedUnitTo(x + 1, y);

                Uscript1.MoveNextTile();

                MMS.ClearSelection();
            }

        }

    }


    void seltectAIPlayerUnits()
    {

        string result = Regex.Match(CurrPlayerCheck, @"\d+").Value;
        Debug.Log("AINum: " + result);
        int playerNum = Int32.Parse(result);

        string PlayerUC = "UnitControllerP";
        string Ucont = string.Concat(PlayerUC, playerNum);
        units = GameObject.FindGameObjectsWithTag(Ucont);

    }


}
