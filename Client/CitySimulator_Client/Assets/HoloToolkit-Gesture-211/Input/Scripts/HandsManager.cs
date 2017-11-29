using UnityEngine.VR.WSA.Input;
using UnityEngine;

namespace Academy.HoloToolkit.Unity
{

    /// <summary>
    /// Module: HandsManager
    /// Team: HoloLens
    /// Description: HandsManager keeps track of when a hand is detected.
    /// Author: 
    ///	 Name:  Microsoft   Date:   Unknown
    /// Modified by:	
    ///	 Name:  Paul McCarlie      Change: Changed interactible to  GUIObjectInteractive Date: 2017-11-25
    /// Based on:  
    /// 	https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_210
    /// </summary>
    public class HandsManager : Singleton<HandsManager>
    {
        [Tooltip("Audio clip to play when Finger Pressed.")]
        public AudioClip FingerPressedSound;
        private AudioSource audioSource;

        /// <summary>
        /// Tracks the hand detected state.
        /// </summary>
        public bool HandDetected
        {
            get;
            private set;
        }

        // Keeps track of the GameObject that the hand is interacting with.
        public GameObject FocusedGameObject { get; private set; }

        void Awake()
        {
            EnableAudioHapticFeedback();

            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += InteractionManager_SourceLost;

            /* TODO: DEVELOPER CODE ALONG 2.a */

            // 2.a: Register for SourceManager.SourcePressed event.
            InteractionManager.SourcePressed += InteractionManager_SourcePressed;

            // 2.a: Register for SourceManager.SourceReleased event.
            InteractionManager.SourceReleased += InteractionManager_SourceReleased;

            // 2.a: Initialize FocusedGameObject as null.
            FocusedGameObject = null;
        }

        private void EnableAudioHapticFeedback()
        {
            // If this hologram has an audio clip, add an AudioSource with this clip.
            if (FingerPressedSound != null)
           {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }

                audioSource.clip = FingerPressedSound;
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1;
                audioSource.dopplerLevel = 0; 
            }
        }

        private void InteractionManager_SourceDetected(InteractionSourceState hand)
        {
            //print("sourceDetected");
            HandDetected = true;
        }

        private void InteractionManager_SourceLost(InteractionSourceState hand)
        {
           // print("sourcelost");
            HandDetected = false;

            // 2.a: Reset FocusedGameObject.
            ResetFocusedGameObject();
        }

        private void InteractionManager_SourcePressed(InteractionSourceState hand)
        {
            print(InteractibleManager.Instance.FocusedGameObject);
            if (InteractibleManager.Instance.FocusedGameObject != null)
            {
                // Play a select sound if we have an audio source and are not targeting an asset with a select sound.
                if (audioSource != null && !audioSource.isPlaying && InteractibleManager.Instance.FocusedGameObject.GetComponent<GUIObjectInteractive>() != null)
                {
                    audioSource.Play();
                }

                // 2.a: Cache InteractibleManager's FocusedGameObject in FocusedGameObject.
                FocusedGameObject = InteractibleManager.Instance.FocusedGameObject;
            }
        }

        private void InteractionManager_SourceReleased(InteractionSourceState hand)
        {
            // 2.a: Reset FocusedGameObject.
            ResetFocusedGameObject();
        }

        private void ResetFocusedGameObject()
        {
            // 2.a: Set FocusedGameObject to be null.
            FocusedGameObject = null;

            // 2.a: On GestureManager call ResetGestureRecognizers
            // to complete any currently active gestures.
            GestureManager.Instance.ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;

            // 2.a: Unregister the SourceManager.SourceReleased event.
            InteractionManager.SourceReleased -= InteractionManager_SourceReleased;

            // 2.a: Unregister for SourceManager.SourcePressed event.
            InteractionManager.SourcePressed -= InteractionManager_SourcePressed;
        }
    }
}