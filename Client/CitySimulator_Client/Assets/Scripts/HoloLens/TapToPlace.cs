using UnityEngine;


/// <summary>
/// Module: TapToPlace
/// Team: HoloLens
/// Description:    This class handles the on select command used by the GazeGestureManager script.
/// Author: 
///	 Name:  Microsoft   Date:   Unknown
/// Modified by:	
///	 Name:  Steven Ma   Change: changed selected object from parent to original object  Date: 2017-10-16
/// Based on:  
/// 	https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_101
/// </summary>
public class TapToPlace : MonoBehaviour
{
    bool placing = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        print("onselect");
        // On each Select gesture, toggle whether the user is in placing mode.
        placing = !placing;
        print(placing);
        // If the user is in placing mode, display the spatial mapping mesh.
        if (placing)
        {
            print("hello");
            SpatialMapping.Instance.DrawVisualMeshes = true;

        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.
       //print("update" + placing);
        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                300.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.position = hitInfo.point;

                // Rotate this object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }
    }
}