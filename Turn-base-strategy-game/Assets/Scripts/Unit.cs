using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //lets the unit know its actual position on the map tiles regardless of its world positions
    public int Xtile;
    public int Ytile;

    public List<TileMap.Node> CurrPath = null;
}
