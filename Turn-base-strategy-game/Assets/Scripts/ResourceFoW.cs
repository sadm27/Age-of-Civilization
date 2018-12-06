using UnityEngine;
using System.Collections;

public class ResourceFoW : MonoBehaviour
{
	private bool observed = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if(observed)
		{
			GetComponentInParent<Animator>().Play("Armature|Eating");
		}
		else
		{
			GetComponentInParent<Animator>().Play("Armature|Stand");
		}

		observed = false;
	}
	
	void Observed()
	{
		observed = true;
        Debug.Log("Chicken Seen!");
	}
}
