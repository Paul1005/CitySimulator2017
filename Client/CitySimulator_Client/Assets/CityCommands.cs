using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        print("onreset");
        // Put the sphere back into its original local position.
        this.transform.localRotation = originalRotation;
    }

}
