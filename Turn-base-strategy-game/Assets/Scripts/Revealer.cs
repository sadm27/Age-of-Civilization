using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revealer : MonoBehaviour {
    public int radius;

    private void Start()
    {
        FogOfWarManager.Instance.RegisterRevealer(this);
    }

    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
		
	}
}
