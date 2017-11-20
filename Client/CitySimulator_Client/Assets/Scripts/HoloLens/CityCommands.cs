using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Module: CityCommands
/// Team: HoloLens
/// Description: Carries out voice commands given by the speech manager.
/// Author: 
///	 Name: Microsoft   Date: Unknown
/// Modified by:	
///	 Name: George Lee   Change: Changed to accept only one command and to reset rotation when it is recieved
/// Based on: 
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class CityCommands : MonoBehaviour {
    Quaternion originalRotation;

    // Use this for initialization
    void Start()
    {
        originalRotation = this.transform.localRotation;
    }

    // Called by SpeechManager when the user says the "Reset world" command
    void OnReset()
    {
        // print("onreset");
        // Put the sphere back into its original local position.
        this.transform.localRotation = originalRotation;
    }

}
