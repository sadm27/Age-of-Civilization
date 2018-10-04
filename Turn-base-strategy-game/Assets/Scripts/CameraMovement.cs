using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 4f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0)
        {
            float movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.Translate(movement, 0, 0);
        }

        if(Input.GetAxis("Vertical") != 0)
        {
            float movement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(0, movement, 0);
        }
	}
}
