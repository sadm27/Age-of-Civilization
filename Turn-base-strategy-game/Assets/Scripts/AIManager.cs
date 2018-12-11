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
    System.Random r = new System.Random();

    //AI just needs to build units

    public GameObject[] units;
    public GameObject unit;
    //Unit[] Uscripts;
    //Unit Uscript;
    public GameObject[] EnemyUnits;
    public GameObject EnemyUnit;

    public string CurrPlayerCheck;
    public string AIPlayerStr;

    // Use this for initialization
    void Start () {

        CurrPlayerCheck = GCScript.GetCurrPlayer();
        seltectAIPlayerUnits();
        seltectAIEnemyUnits();


    }
	
	// Update is called once per frame
	void Update () {

        CurrPlayerCheck = GCScript.GetCurrPlayer();
        AiMoveUnit();
    }













    void AiMoveUnit()
    {
        //Debug.Log("CurrPlayer: " + CurrPlayerCheck);
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString())
        {

            foreach (GameObject unit in units)
            {
                MMS.SelectUnit(unit);
                //Debug.Log("CurrPlayer: " + unit.tag);
                Unit Uscript1 = unit.GetComponent<Unit>();
                int x = Uscript1.Xtile;
                int y = Uscript1.Xtile;

                //check for adjacent tileType to see if it can move there
                AIAttack(Uscript1, x, y);
                
                map.MoveSelectedUnitTo(x + r.Next(-2, 2), y + r.Next(-2, 2));

                Uscript1.MoveNextTile();

                MMS.ClearSelection();
            }
            GCScript.ChangePlayer();

        }

    }



    void AIAttack(Unit Uscript1, int x, int y)
    {

        if (CurrPlayerCheck == AiPlayerScript.tag.ToString())
        {

            foreach (GameObject EnemyUnit in EnemyUnits)
            {
                MMS.SelectUnitEnemy(EnemyUnit);

                Uscript1.attack();

                MMS.ClearSelectionEnemy();
            }

        }
        //check for adjacent tileType to see if it can move there
        map.MoveSelectedUnitTo(x + 1, y);


        

    }



    void seltectAIPlayerUnits()
    {

        string result = Regex.Match(AiPlayerScript.tag, @"\d+").Value;
        Debug.Log("AINum: " + result);
        int playerNum = Int32.Parse(result);

        string PlayerUC = "UnitControllerP";
        string Ucont = string.Concat(PlayerUC, playerNum);
        units = GameObject.FindGameObjectsWithTag(Ucont);

    }




    void seltectAIEnemyUnits()
    {

        //string result = Regex.Match(AiPlayerScript.tag, @"\d+").Value;
        //Debug.Log("AINum: " + result);
        int playerNum = 1; //Int32.Parse(result)

        string PlayerUC = "UnitControllerP";
        string Ucont = string.Concat(PlayerUC, playerNum);
        EnemyUnits = GameObject.FindGameObjectsWithTag(Ucont);

    }



}
