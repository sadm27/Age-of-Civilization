using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {

    //lets the unit know its actual position on the map tiles regardless of its world positions
    public int Xtile;
    public int Ytile;
    TileMap map;
    public bool isGathering = false;
    string UItileType;
    string UITileAmt;
    public int amountGathered = 50;

    public Text UnitName;
    public Text UnitHealth;
    public Text OnTileFood;
    public Text OnTileWood;
    public Text OnTileStone;
    public Text OnTileGold;
    public Text TurnCount;

    public List<Node> CurrPath = null;
    private int moveSpeeds = 2;
    
    public bool isSettler = false;

    void Start()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        Xtile = (int)transform.position.x;
        Ytile = (int)transform.position.y;

        UItileType = map.GetTileResName(Xtile, Ytile);

        if (UItileType == "Wood")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileWood.text = UITileAmt;
        }
        if (UItileType == "Stone")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileStone.text = UITileAmt;
        }
        if (UItileType == "Food")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileFood.text = UITileAmt;
        }
        if (UItileType == "Nothing")
        {
            OnTileWood.text = "0";
            OnTileStone.text = "0";
            OnTileFood.text = "0";
        }

    }


    void Update()
    {
        if(CurrPath != null)
        {
            int CurrNode = 0;
            isGathering = false;
            while (CurrNode < CurrPath.Count - 1)
            {
                //x = NodeX y = NodeY
                Vector3 start = map.TileCoordToWorldCoord(CurrPath[CurrNode].NodeX, CurrPath[CurrNode].NodeY) + new Vector3(0, 0, -1f);
                Vector3 end = map.TileCoordToWorldCoord(CurrPath[CurrNode + 1].NodeX, CurrPath[CurrNode + 1].NodeY) + new Vector3(0, 0, -1f);

                DrawLine(start, end, Color.red);
                //Debug.DrawLine(start, end, Color.red);

                CurrNode++;
            }
        }
        // UnitTile[Xtile, Ytile];
        // UnitName.text = selectedUnit.name;
        // OnTileFood.text = map.GetComponent<TileMap>()map[Xtile,Ytile].amountOfResource;

        UItileType = map.GetTileResName(Xtile, Ytile);

        if (UItileType == "Wood")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileWood.text = UITileAmt;
        }
        if (UItileType == "Stone")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileStone.text = UITileAmt;
        }
        if (UItileType == "Food")
        {
            UITileAmt = map.GetTileResAmt(Xtile, Ytile).ToString();
            OnTileFood.text = UITileAmt;
        }
        if (UItileType == "Nothing")
        {
            OnTileWood.text = "0";
            OnTileStone.text = "0";
            OnTileFood.text = "0";
        }


    

    }


    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }


    


    public void MoveNextTile()
    {
        float remainingMovement = moveSpeeds;



        while (remainingMovement > 0)
        {
            if (CurrPath == null)
            {
                return;
            }

            //gets cost from curr tile to next tile
            remainingMovement -= map.CostToEnterTile(CurrPath[0].NodeX, CurrPath[0].NodeY, CurrPath[1].NodeX, CurrPath[1].NodeY);


            //gets first node and moves us to that position and updates our units world position

            Xtile = CurrPath[1].NodeX;
            Ytile = CurrPath[1].NodeY;
            transform.position = map.TileCoordToWorldCoord(Xtile, Ytile);   //this line can change from transform with animation

            //removing the old current/first node from the path
            CurrPath.RemoveAt(0);

            //no other tiles left on the path so this the the only path so path is cleared
            if (CurrPath.Count == 1)
            {
                CurrPath = null;
                if (map.map[Xtile, Ytile].resource != Tile.tileResource.Nothing)
                {
                    isGathering = true;
                }
            }

        }


    }



}
