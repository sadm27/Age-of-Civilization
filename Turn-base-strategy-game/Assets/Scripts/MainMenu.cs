using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour {

    public TileMap tmScript;

    public Dropdown mapSizeChoice;
    public Button playBtn;

    public float camSpeed = 4;
    Vector3 camPos;
    int xDirection = 1, yDirection = 1;

    float x, y, z;

	// Use this for initialization
	void Start () {
        playBtn.onClick.AddListener(playGame);
	}
	
	// Update is called once per frame
	void Update () {
        camPos = this.gameObject.transform.position;
        x = camPos.x;
        y = camPos.y;

        if(x > 40 || x < 5)
        {
            xDirection *= -1;
        }

        if(y > 20 || y < 5)
        {
            yDirection *= -1;
        }

        transform.Translate(camSpeed * xDirection * Time.deltaTime, camSpeed * yDirection * Time.deltaTime, 0);

	}

    public void playGame() {
        Debug.Log(mapSizeChoice.value);

        switch(mapSizeChoice.value)
        {
            case 0:
                StaticClass.chosenMapSizeX = 20;
                StaticClass.chosenMapSizeY = 20;
                break;

            case 1:
                StaticClass.chosenMapSizeX = 50;
                StaticClass.chosenMapSizeY = 50;
                break;

            case 2:
                StaticClass.chosenMapSizeX = 100;
                StaticClass.chosenMapSizeY = 100;
                break;

            case 3:
                StaticClass.chosenMapSizeX = 200;
                StaticClass.chosenMapSizeY = 200;
                break;

            case 4:
                StaticClass.chosenMapSizeX = 500;
                StaticClass.chosenMapSizeY = 500;
                break;

        }

        SceneManager.LoadScene("Scene1Move", LoadSceneMode.Single);
    }
}
