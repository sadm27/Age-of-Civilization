using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float moveSpeed = 4f;
    public float zoomSpeed = 4f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0)
        {
            float movement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            transform.Translate(movement, 0, 0);
        }

        if(Input.GetAxis("Vertical") != 0)
        {
            float movement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(0, movement, 0);
        }

  
        if(Input.GetAxis("Zoom") != 0) //zooming in/out
        {
            float movement = Input.GetAxis("Zoom") * zoomSpeed * Time.deltaTime;
            transform.Translate(0, 0, movement, Space.Self);
        }
        
	}
}
