using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {

    //prefab class
    public string name;
    public GameObject TileVisualPrefab;

    public bool isWalkable = true;
}
