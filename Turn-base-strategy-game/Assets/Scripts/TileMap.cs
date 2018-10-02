using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {


    public TileType[] tileTypes;

    int[,] tiles;

    int MapSizeX = 200;
    int MapSizeY = 200;

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
                float height = GetHeight(x,y);
                if(height < .35)
                {
                    tiles[x, y] = 3;
                }else if(height < .4)
                {
                    tiles[x, y] = 1;
                }else if(height < .7){
                    tiles[x, y] = 0;
                }
                else { tiles[x, y] = 2; }
            }
        }

    }

    float GetHeight(int x, int y)
    {
        float xCoords = (float)x / MapSizeX * 10;
        float yCoords = (float)y / MapSizeY * 10;

        float sample = Mathf.PerlinNoise(xCoords, yCoords);
        return sample;
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
