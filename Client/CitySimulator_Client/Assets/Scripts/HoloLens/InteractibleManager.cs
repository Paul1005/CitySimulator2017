using Academy.HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// Module: InteractableManager
/// Team: HoloLens
/// Description: InteractibleManager keeps tracks of which GameObject is currently in focus.
/// Author: 
///	 Name:  Microsoft   Date:   Unknown
/// Modified by:	
///	 Name:  Paul McCarlie    Change:Commented out interactible methods Date: 2017-11-25
/// Based on:  
/// 	https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_210
/// </summary>
public class InteractibleManager : Singleton<InteractibleManager>
{
    public GameObject FocusedGameObject { get; private set; }

    private GameObject oldFocusedGameObject = null;

    void Start()
    {
        FocusedGameObject = null;
    }

    void Update()
    {
        oldFocusedGameObject = FocusedGameObject;

        if (GazeManager.Instance.Hit)
        {
            RaycastHit hitInfo = GazeManager.Instance.HitInfo;
            if (hitInfo.collider != null)
            {
                FocusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                FocusedGameObject = null;
            }
        }
        else
        {
            FocusedGameObject = null;
        }
        //print(FocusedGameObject.GetComponent<Interactible>());
       // print(FocusedGameObject);
       /* if (FocusedGameObject != oldFocusedGameObject)
        {
            ResetFocusedInteractible();

            if (FocusedGameObject != null)
            {
                if (FocusedGameObject.GetComponent<Interactible>() != null)
                {
                    FocusedGameObject.SendMessage("GazeEntered");
                }
            }
        }
    }

    private void ResetFocusedInteractible()
    {
        if (oldFocusedGameObject != null)
        {
            if (oldFocusedGameObject.GetComponent<Interactible>() != null)
            {
                oldFocusedGameObject.SendMessage("GazeExited");
            }
        }*/
    }
}