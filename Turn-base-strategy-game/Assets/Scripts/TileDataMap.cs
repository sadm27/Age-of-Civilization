using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum type
    {
        grassland,
        mountain,
        water,
        marsh
    }
    public enum resource
    {
        wood,
        stone,
        food
    }
}

public class TileDataMap  {

    Tile[,] map;

    void Start()
    {
        map = new Tile[50, 50];
    }

}

