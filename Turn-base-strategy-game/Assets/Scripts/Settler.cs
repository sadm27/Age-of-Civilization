using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settler : MonoBehaviour {

    public GameObject cityPrefab;
    public Button settleCityBtn;
    public InputField cityNameChoice;

    MouseManagerS mouseMan;

	// Use this for initialization
	void Start () {
        settleCityBtn.onClick.AddListener(settleCity);
        mouseMan = GameObject.Find("MouseManager").GetComponent<MouseManagerS>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void settleCity()
    {
        if(this.gameObject == mouseMan.selectedUnit)
        {
            GameObject newCity = (GameObject)Instantiate(cityPrefab, this.gameObject.transform.position, Quaternion.Euler (180f, 0f, 0f));
            newCity.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
            City cityScript = newCity.GetComponent<City>();

            if(cityNameChoice.text.ToString() != "")
            {
                cityScript.cityName = cityNameChoice.text.ToString();
            }
            else
            {
                //TODO: random name generator
            }
                
            Destroy(this.gameObject);
        }
    } 


}
