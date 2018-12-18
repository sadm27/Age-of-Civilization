using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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

    //Unit prefabs
    public Button USpawnButt;
    public GameObject EnWarrior;
    public GameObject EnKING;
    public GameObject EnSettler;
    public GameObject EnPezz;

    // Use this for initialization
    void Start () {

        CurrPlayerCheck = GCScript.GetCurrPlayer();
        seltectAIPlayerUnits();
        seltectAIEnemyUnits();
        
        //USpawnButt.onClick.AddListener(SpawnEnemyUnits);


    }
	
	// Update is called once per frame
	void Update () {
        CurrPlayerCheck = GCScript.GetCurrPlayer();

            //SpawnEnWarrior();
            //SpawnEnKING();
            //SpawnEnSettler();
            //SpawnEnPezz();

            AiMoveUnit();


        
    }



    public void SpawnEnWarrior()
    {
        //Debug.Log("Spawn");

        //every five turns spawn unit and resourse check
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString() && GCScript.turnCountNum % 5 == 0 && AiPlayerScript.foodAmount > 100 && AiPlayerScript.woodAmount > 50 && AiPlayerScript.goldAmount > 25)
        {
            //make Vector3 x and y the same as enemy city coordinance or near those coordinance and DO NOT TOUCH the z of vector 3 or the rotation at the end
            Instantiate(EnWarrior, new Vector3(2, 5, 0), Quaternion.Euler(new Vector3(90, 180, 0)));
            AiPlayerScript.foodAmount = AiPlayerScript.foodAmount - 100;
            AiPlayerScript.woodAmount = AiPlayerScript.woodAmount - 50;
            AiPlayerScript.goldAmount = AiPlayerScript.goldAmount - 25;
        }

    }


    public void SpawnEnKING()
    {
        //Debug.Log("Spawn");

        //every five turns spawn unit and resourse check
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString() && GCScript.turnCountNum % 10 == 0 && AiPlayerScript.foodAmount > 200 && AiPlayerScript.woodAmount > 100 && AiPlayerScript.goldAmount > 50)
        {
            //make Vector3 x and y the same as enemy city coordinance or near those coordinance and DO NOT TOUCH the z of vector 3 or the rotation at the end
            Instantiate(EnKING, new Vector3(2, 5, 0), Quaternion.Euler(new Vector3(90, 180, 0)));
            AiPlayerScript.foodAmount = AiPlayerScript.foodAmount - 200;
            AiPlayerScript.woodAmount = AiPlayerScript.woodAmount - 100;
            AiPlayerScript.goldAmount = AiPlayerScript.goldAmount - 50;
        }

    }


    public void SpawnEnSettler()
    {
        //Debug.Log("Spawn");

        //every five turns spawn unit and resourse check
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString() && GCScript.turnCountNum % 15 == 0 && AiPlayerScript.foodAmount > 400 && AiPlayerScript.stoneAmount > 250 && AiPlayerScript.goldAmount > 50)
        {
            //make Vector3 x and y the same as enemy city coordinance or near those coordinance and DO NOT TOUCH the z of vector 3 or the rotation at the end
            Instantiate(EnSettler, new Vector3(2, 5, 0), Quaternion.Euler(new Vector3(90, 180, 0)));
            AiPlayerScript.foodAmount = AiPlayerScript.foodAmount - 400;
            AiPlayerScript.stoneAmount = AiPlayerScript.stoneAmount - 250;
            AiPlayerScript.goldAmount = AiPlayerScript.goldAmount - 50;
        }

    }


    public void SpawnEnPezz()
    {
        //Debug.Log("Spawn");

        //every five turns spawn unit and resourse check
        if (CurrPlayerCheck == AiPlayerScript.tag.ToString() && GCScript.turnCountNum % 5 == 0 && AiPlayerScript.foodAmount > 200 && AiPlayerScript.stoneAmount > 100 && AiPlayerScript.goldAmount > 15)
        {
            //make Vector3 x and y the same as enemy city coordinance or near those coordinance and DO NOT TOUCH the z of vector 3 or the rotation at the end
            Instantiate(EnPezz, new Vector3(2, 5, 0), Quaternion.Euler(new Vector3(90, 180, 0)));
            AiPlayerScript.foodAmount = AiPlayerScript.foodAmount - 200;
            AiPlayerScript.stoneAmount = AiPlayerScript.stoneAmount - 100;
            AiPlayerScript.goldAmount = AiPlayerScript.goldAmount - 15;
        }

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

                int ranX = r.Next(-2, 2);
                int ranY = r.Next(-2, 2);

                if (x + ranX <= map.MapSizeX && x + ranX >= 0 && y + ranY <= map.MapSizeY && y + ranY >= 0)
                {
                    map.MoveSelectedUnitTo(x + r.Next(-2, 2), y + r.Next(-2, 2));

                    Uscript1.MoveNextTile();
                }

                

                MMS.ClearSelection();
            }

            SpawnEnWarrior();
            SpawnEnKING();
            SpawnEnSettler();
            SpawnEnPezz();

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









//Warrior and KING and settler and pezz
//Instantiate(enemy, new Vector3(2, 5, 0), Quaternion.Euler(new Vector3(90, 180, 0)));

//Instantiate(tr.ResourceVisualPrefab, new Vector3(x, y, -.5f), Quaternion.Euler(0, 0, Random.Range(0f, 360f)), this.transform);