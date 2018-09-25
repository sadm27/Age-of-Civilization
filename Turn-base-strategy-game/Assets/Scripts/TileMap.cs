using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {


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





        void generateMapVisuals()
    {

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                Instantiate(tt.TileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );
            }
        }

    }
}
