﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Module: GridManager
/// Team: Client
/// Description: creates 3D grid based on data from CityDataManager
/// Author: 
///	 Name: Dongwon(Shawn) Kim    Date: 2017-09-18
/// Modified by:	
///	 Name: Dongwon(Shawn) Kim   Change:	Fix bug 						Date: 2017-09-19
///  Name: Dongwon(Shawn) Kim   Change:	adding data by CityDataManager  Date: 2017-10-18
///  Name: Dongwon(Shawn) Kim   Change:	turnGrid function        		Date: 2017-11-13
///  Name: Dongwon(Shawn) Kim   Change:	Changed this from Creation to GridManager		Date: 2017-11-28
///  Name: Paul McCarlie        Change: Changed grid rotation point to center   Date: 2017-12-02
/// Based on:  
/// 	https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
/// 	http://answers.unity3d.com/questions/718778/trying-to-create-a-grid.html
/// 	https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
/// 	http://answers.unity3d.com/questions/25352/add-texture-to-gameobject-through-code.html
/// 	https://www.youtube.com/watch?v=810UVUWGlWw
/// </summary>
public class GridManager : MonoBehaviour {

	// 3D prefab of grid
	public Transform cellPrefab;

	// Size for the cell
	public Vector3 size;

	// texture for road
	public Texture roadTexture;
	
	// grid system switch
	public bool turnOnGrid = true;
	public bool turnOffGrid;

	// Reference for the CityDataManager class
	public CityDataManager cityDataManager;
	public CityDataManagerClientOnly cityDataManagerClientOnly;

	// Parent grid object to organize the object in Hierarchy
	public GameObject parentGrid;

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		//ShowGrid (false);
		if(turnOnGrid){
			turnEntireGrid(true);
			turnOnGrid=false;
		} else if(turnOffGrid) {
			turnEntireGrid(false);
			turnOffGrid=false;
		}
	}

	/// <summary>
	/// Creates the grid.
	/// </summary>
	public bool createEntireGridForClientOnly(){
		size.x = cityDataManagerClientOnly.Size_x;
		size.z = cityDataManagerClientOnly.Size_z;

		for(int x = 0; x < size.x; x++){
			for(int z = 0; z < size.z; z++){
				string type = cityDataManagerClientOnly.getIndexOfXZ(x, z).ToString();

				// apply text to the each plane
				cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = type;
				cellPrefab.GetChild (1).GetComponent<TextMesh> ().text = "(" + x + ", " + z + ")";				

				// put the tag plane on the object
				cellPrefab.tag = "plane";
				// set color index to GridColor to color the grid
				cellPrefab.GetComponent<GridColor> ().colorIndex = int.Parse(type);
				
				MeshRenderer component = cellPrefab.GetComponent<MeshRenderer>();
                // component.material.mainTexture = roadTexture;

                //coordinates represent front left corner of the grid
                float positionX = x + (cellPrefab.localScale.x * x) * 10;
                float positionZ = z + (cellPrefab.localScale.z * z) * 10;

                // creates each cell of the grid, will create grid with pivot point at center
                Instantiate(cellPrefab, 
							new Vector3(
                            //positions x-coordinate of the cell so grid will rotate around the center
                            positionX - size.x * 2.7f,
							0,
                            //positions z-coordinate of the cell so grid will rotate around the center
                            positionZ - size.z * 2.6f),
							Quaternion.identity,
							parentGrid.transform);
			}
		}

		turnEntireGrid(true);

		return true;
	}


	/// <summary>
	/// Creates the grid.
	/// </summary>
	public bool createEntireGrid(){
		size.x = cityDataManager.Size_x;
		size.z = cityDataManager.Size_z;

		for(int x = 0; x < size.x; x++){
			for(int z = 0; z < size.z; z++){
				string type = cityDataManager.getIndexOfXZ(x, z).ToString();

				// apply text to the each plane
				cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = type;
				cellPrefab.GetChild (1).GetComponent<TextMesh> ().text = "(" + x + ", " + z + ")";				

				// put the tag plane on the object
				cellPrefab.tag = "plane";
				// set color index to GridColor to color the grid
				cellPrefab.GetComponent<GridColor> ().colorIndex = int.Parse(type);
				
				MeshRenderer component = cellPrefab.GetComponent<MeshRenderer>();
				// component.material.mainTexture = roadTexture;

				// creates each cell of the grid
			 	Instantiate(cellPrefab, 
							new Vector3(
							x + (cellPrefab.localScale.x * x)*8,
							0,
							z + (cellPrefab.localScale.z * z)*8),
							Quaternion.identity,
							parentGrid.transform);
			}
		}

		turnEntireGrid(true);

		return true;
	}

	/// <summary>
	/// Update the grid.
	/// </summary>
	public bool updateEntireGridForClient(){

		GameObject[] planes = GameObject.FindGameObjectsWithTag("plane");
		Debug.Log("Destory: " + planes.Length);
		foreach(GameObject plane in planes)
			Destroy(plane);
		
		createEntireGridForClientOnly();
		
		return true;
	}

	/// <summary>
	/// Update the grid.
	/// </summary>
	public bool updateEntireGrid(){

		GameObject[] planes = GameObject.FindGameObjectsWithTag("plane");
		Debug.Log("Destory: " + planes.Length);
		
		foreach(GameObject plane in planes)
			Destroy(plane);

		createEntireGrid();
		
		return true;
	}

	/// <summary>
	/// Shows the grid.
	/// </summary>
	public void turnEntireGrid(bool on){

		// important this will inactivate all grid objects, so the building and other objects will not be rendered.
		// parentGrid.SetActive(onOff);
		GameObject[] planes = GameObject.FindGameObjectsWithTag ("plane");

		foreach (GameObject plane in planes) {
			Transform planeTransform = plane.transform;
			string type = planeTransform.GetChild(0).GetComponent<TextMesh>().text;


			if(type == "0"){
				MeshRenderer component = plane.GetComponent<MeshRenderer>();
				component.material.mainTexture = roadTexture;
				component.enabled = true;
			} else if ( type != "0"){
				MeshRenderer component = plane.GetComponent<MeshRenderer>();
				component.enabled = on;
			} 
			planeTransform.GetChild (0).GetComponent<MeshRenderer>().enabled = on;
			planeTransform.GetChild (1).GetComponent<MeshRenderer>().enabled = on;

		}
	}

}
