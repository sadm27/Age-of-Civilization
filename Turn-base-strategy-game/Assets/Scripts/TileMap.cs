using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;

    int MapSizeX = 10;
    int MapSizeY = 10;

    void Start()
    {


        generateMap();
        generateMapVisuals();
    }





    void generateMap()
    {
        //allocation of map tiles
        tiles = new int[MapSizeX, MapSizeY];



        //initialize map tiles
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        tiles[4, 4] = 2;
        tiles[5, 4] = 1;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;

        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }




    //creates tiles visually on the map
        void generateMapVisuals()
    {

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                //tile type of the tiles coordinace is set so the tile can call the correct visual prefab

                GameObject go = (GameObject)Instantiate(tt.TileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );
                //the game object is initalized and created at set coordinace

                tileClicker CT = go.GetComponent<tileClicker>();
                //gets components of the tile game object and sets them to the tile clicker to help that

                CT.Xtile = x;
                CT.Ytile = y;
                CT.map = this;
            }
        }

    }

    //get the tile coordinace on the map and relates that to the worlds coordinace
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    //sets the units data on what tile it is on and then set it up visually
    public void MoveSelectedUnitTo(int x, int y)
    {
        selectedUnit.GetComponent<Unit>().Xtile = x;
        selectedUnit.GetComponent<Unit>().Ytile = y;
        selectedUnit.transform.position = TileCoordToWorldCoord(x,y);
    }

}
