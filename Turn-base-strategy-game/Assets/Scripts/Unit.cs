using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //lets the unit know its actual position on the map tiles regardless of its world positions
    public int Xtile;
    public int Ytile;
    TileMap map;
    public bool isGathering = true;
    public bool canSettleCity = false;
    public GameObject cityPrefab;

    public List<Node> CurrPath = null;
    private int moveSpeeds = 2;

    MouseManagerS mouseMan;


    void Start()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        Xtile = (int)transform.position.x;
        Ytile = (int)transform.position.y;
        mouseMan = GameObject.Find("MouseManager").GetComponent<MouseManagerS>();
    }


    void Update()
    {
        if(canSettleCity && mouseMan.selectedUnit == this.gameObject && Input.GetKeyDown(KeyCode.F))
        {
            SettleCity();
        }


        if(CurrPath != null)
        {
            int CurrNode = 0;
            isGathering = false;
            while (CurrNode < CurrPath.Count - 1)
            {
                //x = NodeX y = NodeY
                Vector3 start = map.TileCoordToWorldCoord(CurrPath[CurrNode].NodeX, CurrPath[CurrNode].NodeY) + new Vector3(0, 0, -1f);
                Vector3 end = map.TileCoordToWorldCoord(CurrPath[CurrNode + 1].NodeX, CurrPath[CurrNode + 1].NodeY) + new Vector3(0, 0, -1f);


                Debug.DrawLine(start, end, Color.red);

                CurrNode++;
            }
        }
    }


    void SettleCity()
    {
        Instantiate(cityPrefab, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -0.9f), Quaternion.Euler(90, 0, 0));
        Destroy(gameObject);
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
