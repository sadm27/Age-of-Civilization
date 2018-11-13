using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public int foodAmount = 500;
    public int stoneAmount = 500;
    public int woodAmount = 500;
    public int goldAmount = 500;
    public bool canMoveUnits = false;

    public Text UIplayerFood;
    public Text UIplayerStone;
    public Text UIplayerWood;
    public Text UIplayerGold;

    // Use this for initialization
    void Start() {
        UIplayerFood.text = foodAmount.ToString() + " Food";
        UIplayerStone.text = stoneAmount.ToString() + " Stone";
        UIplayerWood.text = woodAmount.ToString() + " Wood";
        UIplayerGold.text = goldAmount.ToString() + " Gold";
    }




	
	// Update is called once per frame
	void Update () {
        UIplayerFood.text = foodAmount.ToString() + " Food ";
        UIplayerStone.text = stoneAmount.ToString() + " Stone ";
        UIplayerWood.text = woodAmount.ToString() + " Wood ";
        UIplayerGold.text = goldAmount.ToString() + " Gold ";
    }

    public void gatherResource()
    {

    }
}
