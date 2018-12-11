using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MouseManagerS : MonoBehaviour {

    public TileMap map;
    public GameController GC;

    public GameObject selectedUnit;
    public GameObject enemySelectedUnit;
    public GameObject UnitInfo;

    string UItileType;
    string UITileAmt;

    public Text UnitName;
    public Text UnitHealth;
    public Text OnTileFood;
    public Text OnTileWood;
    public Text OnTileStone;
    public Text OnTileGold;
    public Text TurnCount;
    public int turnCountNum;
    public string MMCurrPlayer;


    

    // Use this for initialization
    void Start ()
    {
        UIAssingment();
    }


    // Update is called once per frame
    void Update () {
        UIAssingment();
        PlayerSelectUnit();
        selectEnemy();


    }


    void UIAssingment()
    {
        if(selectedUnit != null){ 

        Unit CurrUnitS = selectedUnit.GetComponent<Unit>();

        UnitName.text = CurrUnitS.UnitName;

        UnitHealth.text = CurrUnitS.HP.ToString();

        string UItileType = map.GetTileResName(CurrUnitS.Xtile, CurrUnitS.Ytile);


        if (UItileType == "Wood")
        {
            UITileAmt = map.GetTileResAmt(CurrUnitS.Xtile, CurrUnitS.Ytile).ToString();
            OnTileStone.text = "0";
            OnTileGold.text = "0";
            OnTileFood.text = "0";
            OnTileWood.text = UITileAmt;
        }
        if (UItileType == "Stone")
        {
            UITileAmt = map.GetTileResAmt(CurrUnitS.Xtile, CurrUnitS.Ytile).ToString();
            OnTileStone.text = UITileAmt;
            OnTileGold.text = "0";
            OnTileFood.text = "0";
            OnTileWood.text = "0";
        }
        if (UItileType == "Food")
        {
            UITileAmt = map.GetTileResAmt(CurrUnitS.Xtile, CurrUnitS.Ytile).ToString();
            OnTileFood.text = UITileAmt;
            OnTileStone.text = "0";
            OnTileGold.text = "0";
            OnTileWood.text = "0";
        }
        if (UItileType == "Gold")
        {
            UITileAmt = map.GetTileResAmt(CurrUnitS.Xtile, CurrUnitS.Ytile).ToString();
            OnTileGold.text = UITileAmt;
            OnTileFood.text = "0";
            OnTileStone.text = "0";
            OnTileWood.text = "0";
        }
        if (UItileType == "Nothing")
        {
            OnTileWood.text = "0";
            OnTileStone.text = "0";
            OnTileFood.text = "0";
            OnTileGold.text = "0";
        }

        }
    }


     public void SelectUnit(GameObject obj) {
		if(selectedUnit != null) {
			if(obj == selectedUnit)
				return;

			ClearSelection();
		}

        selectedUnit = obj;

		Renderer[] rs = selectedUnit.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.green;
			r.material = m;
		}
	}



    public void ClearSelection()
    {
        if (selectedUnit == null)
            return;

        Renderer[] rs = selectedUnit.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = Color.white;
            r.material = m;
        }

        OnTileWood.text = "0";
        OnTileStone.text = "0";
        OnTileFood.text = "0";
        OnTileGold.text = "0";
        //Debug.Log("THE RESORCES SOULD BE CLEARD OUT!");

        selectedUnit = null;
    }



    public void SelectUnitEnemy(GameObject obj)
    {
        if (enemySelectedUnit != null)
        {
            if (obj == enemySelectedUnit)
                return;

            ClearSelectionEnemy();
        }

        enemySelectedUnit = obj;

        Renderer[] rs = enemySelectedUnit.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = Color.red;
            r.material = m;
        }
    }



    public void ClearSelectionEnemy() {
		if(enemySelectedUnit == null)
			return;

		Renderer[] rs = enemySelectedUnit.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.white;
			r.material = m;
		}

        OnTileWood.text = "0";
        OnTileStone.text = "0";
        OnTileFood.text = "0";
        OnTileGold.text = "0";
       //Debug.Log("THE RESORCES SOULD BE CLEARD OUT!");

        enemySelectedUnit = null;
	}




    public void RunMoveNext()
    {
        GameObject[] units;


        string result = Regex.Match(MMCurrPlayer, @"\d+").Value;
        //Debug.Log("Num: " + result);
        int playerNum = Int32.Parse(result);

        string PlayerUC = "UnitControllerP";
        string Ucont = string.Concat(PlayerUC, playerNum);

        units = GameObject.FindGameObjectsWithTag(Ucont);

        foreach (GameObject unit in units)
        {
            Unit Uscript = unit.GetComponent<Unit>();
            Uscript.NumOfAttacksThisTurn = 0;

            SelectUnit(unit);
            selectedUnit.GetComponent<Unit>().MoveNextTile();
        }
        gatherResources();
    }



    void PlayerSelectUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MMCurrPlayer = GC.GetCurrPlayer();

            //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hitInfo = new RaycastHit();


            //Debug.Log("Num1: " + MMCurrPlayer);
            string result = Regex.Match(MMCurrPlayer, @"\d+").Value;
            //Debug.Log("Num: " + result);
            int playerNum = Int32.Parse(result);

            string PlayerUT = "Unit tag P";
            string UTag = string.Concat(PlayerUT, playerNum);

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {



                if (hitInfo.transform.gameObject.tag == UTag)
                {

                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    //Debug.Log("tile" + hitInfo.transform.gameObject.name);

                    SelectUnit(hitObject);
                    UnitInfo.gameObject.SetActive(true);
                }

            }
            else
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    ClearSelection();
                    UnitInfo.gameObject.SetActive(false);
                }
            }

        }
        if (Input.GetButtonDown("Cancel"))
        {
            ClearSelection();
            UnitInfo.gameObject.SetActive(false);
        }
    }


    void selectEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MMCurrPlayer = GC.GetCurrPlayer();

            //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hitInfo = new RaycastHit();


            //Debug.Log("Num1: " + MMCurrPlayer);
            string result = Regex.Match(MMCurrPlayer, @"\d+").Value;
            //Debug.Log("Num: " + result);
            int playerNum = Int32.Parse(result);

            string PlayerUT = "Unit tag P";
            string UTag = string.Concat(PlayerUT, playerNum);

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {



                if (hitInfo.transform.gameObject.tag != UTag && hitInfo.transform.gameObject.tag.Contains(PlayerUT))
                {

                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    //Debug.Log("Etile" + hitInfo.transform.gameObject.name);

                    SelectUnitEnemy(hitObject);
                    UnitInfo.gameObject.SetActive(true);

                    if(selectedUnit != null)
                    {
                        Unit Uscript = selectedUnit.GetComponent<Unit>();

                        Uscript.attack();
                    }

                    

                }

            }
            else
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    ClearSelectionEnemy();
                    UnitInfo.gameObject.SetActive(false);
                }
            }

        }
        if (Input.GetButtonDown("Cancel"))
        {
            ClearSelectionEnemy();
            UnitInfo.gameObject.SetActive(false);
        }
    }


    public void gatherResources()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        int Xtile;
        int Ytile;


        GameObject[] units;
        GameObject player;

        //buffer get text on screen and parses it for the first int if there is no in an error will be thrown
        string result = Regex.Match(MMCurrPlayer, @"\d+").Value;
        //Debug.Log("Num: " + result);
        int playerNum = Int32.Parse(result);

        string PlayerUC = "UnitControllerP";
        string Ucont = string.Concat(PlayerUC, playerNum);

        player = GameObject.FindGameObjectWithTag(MMCurrPlayer);
        units = GameObject.FindGameObjectsWithTag(Ucont);
        

        //units = GameObject.FindGameObjectsWithTag("UnitController");
        //player = GameObject.FindGameObjectWithTag("Player1");
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Unit>().isGathering == true)
            {
                Xtile = (int)unit.transform.position.x;
                Ytile = (int)unit.transform.position.y;

                int amountOnTile = map.GetTileResAmt(Xtile, Ytile);
                if(amountOnTile > unit.GetComponent<Unit>().amountGathered)
                {
                    map.gatherResource(Xtile, Ytile, unit.GetComponent<Unit>().amountGathered);
                    string type = map.GetTileResName(Xtile, Ytile);
                    switch (type)
                    {
                        case "Wood":
                            player.GetComponent<Player>().woodAmount += unit.GetComponent<Unit>().amountGathered;
                            break;

                        case "Food":
                            player.GetComponent<Player>().foodAmount += unit.GetComponent<Unit>().amountGathered;
                            break;

                        case "Stone":
                            player.GetComponent<Player>().stoneAmount += unit.GetComponent<Unit>().amountGathered;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    map.gatherResource(Xtile, Ytile, amountOnTile);
                    string type = map.GetTileResName(Xtile, Ytile);
                    switch (type)
                    {
                        case "Wood":
                            player.GetComponent<Player>().woodAmount += amountOnTile;
                            break;

                        case "Food":
                            player.GetComponent<Player>().foodAmount += amountOnTile;
                            break;

                        case "Stone":
                            player.GetComponent<Player>().stoneAmount += amountOnTile;
                            break;

                        default:
                            break;
                    }
                    map.removeResource(Xtile, Ytile);
                }
            }
        }
    }








}
