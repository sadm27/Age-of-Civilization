using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MouseManagerS : MonoBehaviour {


    public GameObject selectedUnit;
    public GameObject UnitInfo;

    public Text UnitName;
    public Text UnitHealth;
    public Text OnTileFood;
    public Text OnTileWood;
    public Text OnTileStone;
    public Text OnTileGold;
    public Text TurnCount;
    public int turnCountNum;


    public TileMap map;

    // Use this for initialization
    void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {

            //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hitInfo = new RaycastHit();

            int trunCOU = getTurnCountNum();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {
                
                if (hitInfo.transform.gameObject.tag == "Unit tag P1" && trunCOU % 2 == 1)
                {
                    
                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    Debug.Log("tile" + hitInfo.transform.gameObject.name);

                    SelectUnit(hitObject);
                    UnitInfo.gameObject.SetActive(true);
                }

                if (hitInfo.transform.gameObject.tag == "Unit tag P2" && trunCOU % 2 == 0)
                {

                    GameObject hitObject = hitInfo.transform.root.gameObject;
                    Debug.Log("tile" + hitInfo.transform.gameObject.name);

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


    void SelectUnit(GameObject obj) {
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

	void ClearSelection() {
		if(selectedUnit == null)
			return;

		Renderer[] rs = selectedUnit.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.white;
			r.material = m;
		}

        OnTileWood.text = "0";
        OnTileStone.text = "0";
        OnTileFood.text = "0";
        Debug.Log("THE RESORCES SOULD BE CLEARD OUT!");

        selectedUnit = null;
	}



    public void Turncounter()
    {
        turnCountNum++;
        TurnCount.text = string.Concat("Turn: ", turnCountNum);
    }




    public void RunMoveNext()
    {
        int trunCOU = getTurnCountNum();
        GameObject[] units;
        //units = GameObject.FindGameObjectsWithTag("UnitController");

        if (trunCOU % 2 == 1)
        {
            units = GameObject.FindGameObjectsWithTag("UnitControllerP1");
        }
        else
        {
            //if (trunCOU % 2 == 0)
            units = GameObject.FindGameObjectsWithTag("UnitControllerP2");
        }

        foreach (GameObject unit in units)
        {
            SelectUnit(unit);
            selectedUnit.GetComponent<Unit>().MoveNextTile();
        }
        gatherResources();
    }



    public int getTurnCountNum()
    {
        string buff = TurnCount.text;

        //buffer get text on screen and parses it for the first int if there is no in an error will be thrown
        string result = Regex.Match(buff, @"\d+").Value;

        return Int32.Parse(result);
    }


    public void gatherResources()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        int Xtile;
        int Ytile;


        GameObject[] units;
        GameObject player;
        int trunCOU = getTurnCountNum();
        

        if (trunCOU % 2 == 1)
        {
            player = GameObject.FindGameObjectWithTag("Player1");
            units = GameObject.FindGameObjectsWithTag("UnitControllerP1");
        }
        else
        {
            //if (trunCOU % 2 == 0)
            player = GameObject.FindGameObjectWithTag("Player2");
            units = GameObject.FindGameObjectsWithTag("UnitControllerP2");
        }


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
