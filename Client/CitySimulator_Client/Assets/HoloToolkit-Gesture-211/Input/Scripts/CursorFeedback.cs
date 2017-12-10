﻿using UnityEngine;

namespace Academy.HoloToolkit.Unity
{
    /// <summary>
    /// Module: CursorFeedback
    /// Team: HoloLens
    /// Description: CursorFeedback class takes GameObjects to give cursor feedback to users based on different states.
    /// Author:
    ///	 Name: Microsoft   Date: Unknown
    ///	Modified by:
    ///	 Name Paul McCarlie Change: Added booleon to switch scroll feedback on and off Date: 2017-11-28
    /// Based on:
    /// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_211
    /// </summary>
    public class CursorFeedback : MonoBehaviour
    {
        [Tooltip("Drag a prefab object to display when a hand is detected.")]
        public GameObject HandDetectedAsset;
        private GameObject handDetectedGameObject;

        [Tooltip("Drag a prefab object to display when a scroll enabled Interactible is detected.")]
        public GameObject ScrollDetectedAsset;
        private GameObject scrollDetectedGameObject;

        [Tooltip("Drag a prefab object to display when a pathing enabled Interactible is detected.")]
        public GameObject PathingDetectedAsset;
        private GameObject pathingDetectedGameObject;

        [Tooltip("Drag a prefab object to parent the feedback assets.")]
        public GameObject FeedbackParent;

        // determines whether scroll feedback is enabled
        public bool scrollEnabled;

        private GUIObjectInteractive FocusedInteractible
        {
            get
            {
                if (InteractibleManager.Instance.FocusedGameObject != null)
                {
                    return InteractibleManager.Instance.FocusedGameObject.GetComponent<GUIObjectInteractive>();
                }

                return null;
            }
        }

        void Awake()
        {
            if (HandDetectedAsset != null)
            {
                handDetectedGameObject = InstantiatePrefab(HandDetectedAsset);
            }

            if (ScrollDetectedAsset != null)
            {
                scrollDetectedGameObject = InstantiatePrefab(ScrollDetectedAsset);
            }

            if (PathingDetectedAsset != null)
            {
                pathingDetectedGameObject = InstantiatePrefab(PathingDetectedAsset);
            }
        }

        private GameObject InstantiatePrefab(GameObject inputPrefab)
        {
            GameObject instantiatedPrefab = null;

            if (inputPrefab != null && FeedbackParent != null)
            {
                instantiatedPrefab = GameObject.Instantiate(inputPrefab);
                // Assign parent to be the FeedbackParent
                // so that feedback assets move and rotate with this parent.
                instantiatedPrefab.transform.parent = FeedbackParent.transform;

                // Set starting state of gameobject to be inactive.
                instantiatedPrefab.gameObject.SetActive(false);
            }

            return instantiatedPrefab;
        }

        void Update()
        {
            UpdateHandDetectedState();

            UpdatePathDetectedState();

            UpdateScrollDetectedState();
        }

        private void UpdateHandDetectedState()
        {
            if (handDetectedGameObject == null || CursorManager.Instance == null)
            {
                return;
            }

            handDetectedGameObject.SetActive(HandsManager.Instance.HandDetected);
        }

        private void UpdatePathDetectedState()
        {
            if (pathingDetectedGameObject == null)
            {
                return;
            }

            if (CursorManager.Instance == null || FocusedInteractible == null ||
                GestureManager.Instance.ActiveRecognizer != GestureManager.Instance.ManipulationRecognizer)
            {
                pathingDetectedGameObject.SetActive(false);
                return;
            }

            pathingDetectedGameObject.SetActive(true);
        }

        private void UpdateScrollDetectedState()
        {
            if (scrollDetectedGameObject == null)
            {
                return;
            }

            if (CursorManager.Instance == null || FocusedInteractible == null ||
                GestureManager.Instance.ActiveRecognizer != GestureManager.Instance.NavigationRecognizer)
            {
                scrollDetectedGameObject.SetActive(false);
                return;
            }

            //Activate scroll feedback only if we are in rotatoin mode
            if (scrollEnabled == true)
            {
                scrollDetectedGameObject.SetActive(true);
            }
        }
    }
}