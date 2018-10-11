using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {


    public GameObject selectedUnit;

    public TileMap map;

    // Use this for initialization
    void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {

            //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {
                
                if (hitInfo.transform.gameObject.tag == "Unit tag")
                {
                    
                    GameObject hitObject = hitInfo.transform.root.gameObject;

                    SelectUnit(hitObject);
                    map.SelectUnit(selectedUnit);

                    
                }
                else
                {
                    ClearSelection();
                }


            }

            else
            {
                ClearSelection();
            }

        }
    }


    void SelectUnit(GameObject obj) {
		if(selectedUnit != null) {
			if(obj == selectedUnit)
				return;

			ClearSelection();
		}

        selectedUnit = obj;

		Renderer[] rs = selectedUnit.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.green;
			r.material = m;
		}
	}

	void ClearSelection() {
		if(selectedUnit == null)
			return;

		Renderer[] rs = selectedUnit.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.white;
			r.material = m;
		}


        selectedUnit = null;
	}


}
