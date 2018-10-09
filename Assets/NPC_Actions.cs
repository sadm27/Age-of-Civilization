using UnityEngine;
using System.Collections;

public class NPC_Actions : MonoBehaviour {

    public float speed = .01f;
    Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.velocity = transform.forward * speed;
	}
}
