using Academy.HoloToolkit.Unity;
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
///	 Name:  George Lee      Change: Changed to accept only one command and to reset rotation when it is recieved Date: 2017-11-01
///	 Name:  George Lee      Change: Added move city command for spacial mapping                                  Date: 2017-11-25
///	 Name:  Paul McCarlie   Change: Added mode switching for roration and selection for the time being           Date: 2017-11-01
/// Based on: 
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class CityCommands : MonoBehaviour
{
    Quaternion originalRotation;
    GestureAction gestureAction;
    GUIMouseEventManager eventManager;
    CursorFeedback cursorFeedback;
    GameObject cursor;
    // Use this for initialization
    private AudioSource audioSource;
    public AudioClip rotateMode;
    public AudioClip selectMode;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        originalRotation = this.transform.localRotation;
        gestureAction = GetComponent<GestureAction>();
        eventManager = GetComponent<GUIMouseEventManager>();
        cursor = GameObject.Find("Cursor");
        cursorFeedback = cursor.GetComponent<CursorFeedback>();
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
        cursorFeedback.scrollEnabled = true;

        audioSource.clip = rotateMode;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

    private void EnableSelection()
    {
        gestureAction.enabled = false;
        eventManager.isEnabled = true;
        cursorFeedback.scrollEnabled = false;

        audioSource.clip = selectMode;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

}
