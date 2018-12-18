using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    public string cityName = "New City";
    public TextMesh CityNameLabel;

	// Use this for initialization
	void Start () {
        CityNameLabel.text = cityName; //set label to city name
	}
	
	// Update is called once per frame
	void Update () {

	}



}
