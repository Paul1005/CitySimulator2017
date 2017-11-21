using UnityEngine;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// Module: GazeGestureManager
/// Team: HoloLens
/// Description: Tracks what object is being tapped on, and sends commands to the appropriate script for spatial mapping
/// Author:
///	 Name: Microsoft   Date: Unknown
///	 Modified by:	
///	 Name:  Paul McCarlie  Change: Modified so that gestures will only register on 2 taps, to prevent confusing it with rotation gestures  Date: 2017-11-01
/// Based on:
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_101
/// </summary>
public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;
    int tapCount = 0;
    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            this.tapCount = tapCount;

            //this is to try and stop the program from getting confused between spacial mapping and rotation gestures
            if (this.tapCount == 2)
            {
                //print("tapcount:" + this.tapCount);
               // print(FocusedObject);
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
                }
            }
        };

        //similar to above version, could maybe try and refactor so repetition is not needed
        if(this.tapCount == 2)
        {
            recognizer.StartCapturingGestures();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
           // print("hit");
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
            //print("miss");
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}