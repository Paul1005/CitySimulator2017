﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Module: BuildingCreation
/// Team: Client
/// Description: Creating building depends on the grid information
/// Author: 
///	 Name: Dongwon(Shawn) Kim   Date: 2017-09-28
///  Name: Andrew Lam			Date: 2017-09-28
/// Modified by:	
///	 Name: Andrew Lam   Change: Change the reference of the building	Date: 2017-10-17
/// Based on:  N/A
/// https://docs.unity3d.com/ScriptReference/Material-color.html
/// </summary>
public class BuildingCreation : MonoBehaviour
{

    public Transform building;
    private GameObject[] planes;
    private GameObject[] buildings;
    private Transform planeTransform;
    private GameObject parent;
    // Use this for initialization
    void Start()
    {
        parent = GameObject.Find("Grid");
        planeTransform = GameObject.Find("Plane(Clone)").transform;
        CreateBuilding();
        ParentBuidlings();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Create building model
    void CreateBuilding()
    {
        planes = GameObject.FindGameObjectsWithTag("plane");
        int x = 0;
        int z = 0;

        building.localScale = planeTransform.localScale;
        building.localScale -= new Vector3(0.15f, 0, 0.15f);

        foreach (GameObject grid in planes)
        {
            if (grid.transform.GetChild(0).GetComponent<TextMesh>().text == "1")
            {
                //Debug.Log("find 1: " + grid.transform.position.x + ", " + grid.transform.position.y);

                building.tag = "building";
                //Creating each cell of grid
                Instantiate(building, new Vector3(grid.transform.position.x, 0, grid.transform.position.z),
                    Quaternion.identity);
            }
           //grid.AddComponent<Interactible>();
            //grid.AddComponent<GazeGestureManager>();
            //grid.AddComponent<TapToPlace>();
            //Set collider properly
            grid.GetComponent<MeshCollider>().convex = true;
            //set parent
            grid.transform.parent = parent.transform;
            z++;
            x++;
        }

        //		if (plane.GetComponentInChildren<TextMesh> ().text == "1") {
        //			Instantiate (building);
        //		}

    }
    void ParentBuidlings()
    {
        buildings = GameObject.FindGameObjectsWithTag("building");
        foreach (GameObject buildingObject in buildings)
        {
            buildingObject.transform.parent = parent.transform;

        }
    }
}
