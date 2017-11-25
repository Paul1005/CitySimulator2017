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
    GestureAction gestureAction;
    GUIMouseEventManager eventManager;
    // Use this for initialization
    private AudioSource audioSource;
    public AudioClip rotateMode;
    public AudioClip selectMode;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalRotation = this.transform.localRotation;
        gestureAction = GetComponent<GestureAction>();
        eventManager = GetComponent<GUIMouseEventManager>();
    }

    // Called by SpeechManager when the user says the "Reset world" command
    void OnReset()
    {
        // print("onreset");
        // Put the sphere back into its original local position.
        this.transform.localRotation = originalRotation;
    }

    void DeSelect()
    {
        List<GUIObjectInteractive> Selections = GUIMouseEventManager.Selections;
        foreach (var sel in Selections)
        {
            if (sel != null) sel.Deselect();
        }
        Selections.Clear();
    }

    void EnableRotation()
    {
        gestureAction.enabled = true;
        eventManager.isEnabled = false;
        audioSource.clip = rotateMode;
        audioSource.Play();
    }

    private void EnableSelection()
    {
        gestureAction.enabled = false;
        eventManager.isEnabled = true;
        audioSource.clip = selectMode;
        audioSource.Play();
    }

}
