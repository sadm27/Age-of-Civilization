using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tileClicker : MonoBehaviour {

    //coordinates of the tile and the map used along with the mouse click to move the unit
    public int Xtile;
    public int Ytile;
    public TileMap map;

    void OnMouseUp()
    {
        //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Debug.Log("click!");

        map.MoveSelectedUnitTo(Xtile, Ytile);
    }
}
