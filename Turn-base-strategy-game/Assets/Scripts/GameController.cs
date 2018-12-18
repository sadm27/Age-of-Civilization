using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int playerCount;
    public Player player_1;
    public Player player_2;
    public Player player_3;
    public Player player_4;
    public Player player_5;
    public Player player_6;
    public Player player_7;
    public Player player_8;
    public Player CurrPlayer;
    public Button nextTurnButton;
    public Text TurnCount;
    public Text CurrenPlayer;
    public int turnCountNum;
    public MouseManagerS MMS;

    public float counter = 0;

    // Use this for initialization
    void Start () {

        CurrPlayer = player_1;

        Debug.Log("Player: " + CurrPlayer.tag);
        CurrenPlayer.text = string.Concat("Current Player: ", CurrPlayer.tag);

        turnCountNum = 1;
        TurnCount.text = string.Concat("Turn: ", turnCountNum);

        nextTurnButton.onClick.AddListener(ChangePlayer);

    }


    public int getTurnCountNum()
    {
        string buff = TurnCount.text;

        //buffer get text on screen and parses it for the first int if there is no in an error will be thrown
        string result = Regex.Match(buff, @"\d+").Value;

        return Int32.Parse(result);
    }


    public void ChangePlayer()
    {
        
        string buff = CurrPlayer.tag;

        //buffer get text on screen and parses it for the first int if there is no in an error will be thrown
        string result = Regex.Match(buff, @"\d+").Value;

        int playerNum = Int32.Parse(result);

        if(playerNum >= playerCount)
        {
            CurrPlayer = player_1;
            CurrenPlayer.text = string.Concat("Current Player: ", CurrPlayer.tag);
            Debug.Log("Player: " + CurrPlayer.tag);
            playerNum = 1;
            turnCountNum++;
            TurnCount.text = string.Concat("Turn: ", turnCountNum);


            MMS.ClearSelection();
            MMS.ClearSelectionEnemy();
        }
        else
        {
            playerNum++;
            string playerString = "Player";
            string NewPlayerString = string.Concat(playerString, playerNum); // This is the line which was changed.

            Console.WriteLine(NewPlayerString);

            GameObject playerGameObj = GameObject.FindGameObjectWithTag(NewPlayerString);

            if (playerGameObj != null)
            {
                CurrPlayer = playerGameObj.GetComponent<Player>();
                Debug.Log("Player: " + CurrPlayer.tag);
                CurrenPlayer.text = string.Concat("Current Player: ", CurrPlayer.tag);
                MMS.ClearSelection();
                MMS.ClearSelectionEnemy();
            }
                        
        }

        
    }


    public string GetCurrPlayer()
    {
        //Debug.Log(CurrPlayer.tag.ToString());
        return CurrPlayer.tag.ToString();
    }


    // Update is called once per frame
    void Update () {


        counter += Time.deltaTime;

        if (counter >= 5)
        {
            if (CurrPlayer.foodAmount > 0 && CurrPlayer.woodAmount > 0 && CurrPlayer.goldAmount > 0 && CurrPlayer.stoneAmount > 0)
            {
                CurrPlayer.foodAmount = CurrPlayer.foodAmount - 1;
                CurrPlayer.stoneAmount = CurrPlayer.stoneAmount - 1;
                CurrPlayer.woodAmount = CurrPlayer.woodAmount - 1;
                CurrPlayer.goldAmount = CurrPlayer.goldAmount - 1;
            }
            

            //RESET Counter
            counter = 0;
        }


    }



}
