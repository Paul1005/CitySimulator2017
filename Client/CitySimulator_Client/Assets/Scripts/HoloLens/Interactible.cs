using UnityEngine;

/// <summary>
/// Module: Interactable
/// Team: HoloLens
/// Description:    The Interactible class flags a Game Object as being "Interactible".
///                 Determines what happens when an Interactible is being gazed at.
/// Author: 
///	 Name:  Microsoft   Date:   Unknown
/// Modified by:	
///	 Name:  Paul McCarlie   Change: Gazing at object will now make tile text visible, and invisible when gazing away    Date: 2017-10-06
///	 Name:  Paul McCarlie   Change: Class is no longer used, most functionality commented out                           Date: 2017-11-26
/// Based on:  
/// 	https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_210
/// </summary>
public class Interactible : MonoBehaviour
{
    [Tooltip("Audio clip to play when interacting with this hologram.")]
    public AudioClip TargetFeedbackSound;
    private AudioSource audioSource;

   // private Material[] defaultMaterials;
   // private TextMesh[] textMesh;

    private bool _Selected = false;   // check if the unit gets selected

    public bool Selected { get { return _Selected; } }

    public bool Swap = false; // to change selection status in editor

    void Start()
    {
        //defaultMaterials = GetComponent<Renderer>().materials;
        //textMesh = GetComponentsInChildren<TextMesh>();
        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }


       // EnableAudioHapticFeedback();
    }

    private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }
    }

    /* TODO: DEVELOPER CODING EXERCISE 2.d */
    /*
    void GazeEntered()
    {
        //print(defaultMaterials.Length);
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
           // print(defaultMaterials[i]);
            //print(defaultMaterials[i].mainTexture);
            // 2.d: Uncomment the below line to highlight the material when gaze enters.
            defaultMaterials[i].SetFloat("_Highlight", 0.25f);
        }
        textMesh[0].characterSize = 2.5f;
        textMesh[1].characterSize = 2.5f;
    }

    void GazeExited()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            // 2.d: Uncomment the below line to remove highlight on material when gaze exits.
            defaultMaterials[i].SetFloat("_Highlight", 0f);
           // textMesh.text = "";
        }
        textMesh[0].characterSize = 0;
        textMesh[1].characterSize = 0;
    }

    void OnSelect()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetFloat("_Highlight", 0.5f);
        }

        // Play the audioSource feedback when we gaze and select a hologram.
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        /* TODO: DEVELOPER CODING EXERCISE 6.a 
        // 6.a: Handle the OnSelect by sending a PerformTagAlong message.
        //this.SendMessage("PerformTagAlong");
    }*/
}