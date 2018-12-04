using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tileClicker : MonoBehaviour {

    //coordinates of the tile and the map used along with the mouse click to move the unit
    public int Xtile;
    public int Ytile;
    public TileMap map;
    public float distanceToAppear;

    Renderer tileRenderer;
    Transform mainCameraTransform;
    bool isVisible;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        tileRenderer = this.GetComponent<Renderer>();
    }

    private void Update()
    {
        disappear();
    }

    void disappear()
    {
        float distance = Vector3.Distance(mainCameraTransform.position, transform.position);

        if (distance < distanceToAppear)
        {
            if (!isVisible)
            {
                tileRenderer.enabled = true;
                isVisible = true;
            }
            else
            {
                tileRenderer.enabled = false;
                isVisible = false;
            }
        }
    }

    void OnMouseUp()
    {
        CallToMakePath();
    }

    void CallToMakePath()
    {
        //if you are having issues with clicking through the UI use this check on you mouse click / or selection functions
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Debug.Log("click!slkjafjskhfkjdlhsakhdfhskfhjsdfjklshjfhskd");

        map.MoveSelectedUnitTo(Xtile, Ytile);
    }







}
