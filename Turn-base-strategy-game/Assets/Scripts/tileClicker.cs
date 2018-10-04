using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileClicker : MonoBehaviour {

    //coordinates of the tile and the map used along with the mouse click to move the unit
    public int Xtile;
    public int Ytile;
    public TileMap map;

    void OnMouseUp()
    {
        Debug.Log("click!");

        map.MoveSelectedUnitTo(Xtile, Ytile);
    }
}
