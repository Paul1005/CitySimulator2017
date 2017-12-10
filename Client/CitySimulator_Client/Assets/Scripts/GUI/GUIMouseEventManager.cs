using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// Module: GUIMouseEventManager
/// Team: Client
/// Description: Add functionality for mouse clicking, in order to select objects
/// Author: 
///     Name: Benjamin Hao Date: 2017-10-24
/// Modified by:    
///     Name: Benjamin Hao   Change: added skip functionality to increase performance    Date: 2017-10-30
///     Name: Benjamin Hao   Change: added Shift key functionality: Mutipul Select       Date: 2017-10-30
///     Name: Paul McCarlie  Change: add air tap functionality for HoloLens              Date: 2017-11-24
///     Name: Benjamin Hao   Change: added Control key functionality: Zoom in            Date: 2017-11-29
///     
/// Based on: https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html
///           https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
///           GestureManager.cs
/// </summary>

public class GUIMouseEventManager : MonoBehaviour
{
    public static List<GUIObjectInteractive> Selections = new List<GUIObjectInteractive>();  // the list of selected objects

    // Tap gesture recognizer.
    public GestureRecognizer gestureRecognizer { get; private set; }


    private RaycastHit hit;
    // Following is for double click functionality
    public float moveSpeed = 1.0f;

    //used to enable and disable air tap functionality when switching modes
    public bool isEnabled;

    //Use for capturing HoloLens gesture
    private void Awake()
    {
        // 2.b: Instantiate the NavigationRecognizer.
        gestureRecognizer = new GestureRecognizer();

        // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
        gestureRecognizer.SetRecognizableGestures(
            GestureSettings.Tap);
        gestureRecognizer.StartCapturingGestures();
        // 2.b: Register for the TappedEvent.
        gestureRecognizer.TappedEvent += (source, tapCount, ray) =>
        {
            //if in selection mode
            if (isEnabled)
            {
                //object the user is looking at
                GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;
                var interact = focusedObject.transform.GetComponent<GUIObjectInteractive>(); 
                // Check "GUIObjectInteractive" module, if Null, then don't proceed
                if (interact != null)
                {
                    if (Selections.Count > 0)
                    {
                        foreach (var sel in Selections)
                        {
                            if (sel != null) sel.Deselect();
                        }
                        Selections.Clear();
                    }
                    Selections.Add(interact);
                    interact.Select();
                }
            }
        };
    }
 //TODO, implement update functionality into awake
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // If there's no clicking, then skip. To increase performance.
        {
            var es = UnityEngine.EventSystems.EventSystem.current;
            if (es != null && es.IsPointerOverGameObject()) // If the user click 2D objects(such as UI), then need to avoid selecting 3D objects.
                                                            // Meanwhile, we need to check if the user clicked 2D objects
                return;

            if (Selections.Count > 0)
            {
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))   // Remove the objects selected by right click and Shift key
                {
                    foreach (var sel in Selections)
                    {
                        if (sel != null) sel.Deselect();
                    }
                    Selections.Clear();
                }
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))  // If nothing is clicked, return
                return;

            var interact = hit.transform.GetComponent<GUIObjectInteractive>();
            if (interact == null) // Check "Interactive" module, if Null, then return
                return;

            Selections.Add(interact);
            interact.Select();
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl))
            Camera.main.transform.position = Vector3.Lerp(transform.position, hit.transform.position, moveSpeed * Time.deltaTime);
    }
}