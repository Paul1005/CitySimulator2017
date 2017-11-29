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
///	 Name:  Paul McCarlie   Change: Added mode switching for rotation and selection                              Date: 2017-11-25
///	 Name:  Paul McCarlie   Change: Added voice feedback for rotation and selection                              Date: 2017-11-26
///	 Name:  Paul McCarlie   Change: Enable and disable scroll feedback                                           Date: 2017-11-28
/// Based on: 
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class CityCommands : MonoBehaviour
{
    //Stores the city's original orientation
    Quaternion originalRotation;

    //Stores gesture action script for rotation
    GestureAction gestureAction;

    //Stores the GUIMouseEventManager script for selection
    GUIMouseEventManager eventManager;

    //Stores the cursor feedback script
    CursorFeedback cursorFeedback;

    //Stores the cursor
    GameObject cursor;

    //Audo files needed for voice feedback
    private AudioSource audioSource;
    public AudioClip rotateMode;
    public AudioClip selectMode;

    // Use this for initialization
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

    /// <summary>
    /// Called by SpeechManager when the user says the "Reset world" command
    /// </summary>
    void OnReset()
    {
        // print("onreset");
        // Put the sphere back into its original local position.
        this.transform.localRotation = originalRotation;
    }

    /// <summary>
    /// Un selects the currently selected buildings when the users says "Clear selection"
    /// </summary>
    void DeSelect()
    {
        List<GUIObjectInteractive> Selections = GUIMouseEventManager.Selections;
        foreach (var sel in Selections)
        {
            if (sel != null) sel.Deselect();
        }
        Selections.Clear();
    }

    /// <summary>
    /// Will put the user in rotation mode, so they can rotate the city but not select buildings, will also clear any previous selection.
    /// </summary>
    void EnableRotation()
    {
        print("rotation");
        //unselect currently selected building
        DeSelect();

        //enable rotation script
        gestureAction.enabled = true;
        //disable building selection 
        eventManager.isEnabled = false;
        //enable scroll feedback
        cursorFeedback.scrollEnabled = true;

        //play audio feedback to let user konw rotation mode is enable
        audioSource.clip = rotateMode;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

    /// <summary>
    /// Will put the user in selection mode, so they can select buildings but not rotate the city.
    /// </summary>
    private void EnableSelection()
    {
        //disable rotation script
        gestureAction.enabled = false;
        //disable building selection
        eventManager.isEnabled = true;
        //disable scroll feedback
        cursorFeedback.scrollEnabled = false;

        //play audio feedback to let user konw selection mode is enable
        audioSource.clip = selectMode;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

}
