using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Module: SpeechManager
/// Team: HoloLens
/// Description: Handles the speech commands available to the user.
/// Author:
///	 Name:  Microsoft   Date:   Unknown
/// Modified by:
///	 Name:  George Lee      Change: Stripped class down to accept only one command for the time being      Date: 2017-11-01
///	 Name:  George Lee      Change: Added move city command for spacial mapping                            Date: 2017-11-25
///	 Name:  Paul McCarlie   Change: Added mode switching for rotation and selection                        Date: 2017-11-01
/// Based on:
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class SpeechManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer = null;

    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    private void Start()
    {
        // add all the keywords
        keywords.Add("Reset world", () =>
        {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("OnReset");
        });
        keywords.Add("Clear", () =>
        {
            // Call the DeSelect method on every descendant object.
            this.BroadcastMessage("DeSelect");
        });
        keywords.Add("Rotation mode", () =>
        {
            // Call the Enable Rotation method on every descendant object.
            this.BroadcastMessage("EnableRotation");
        });
        keywords.Add("Selection mode", () =>
        {
            // Call the Enable Selection method on every descendant object.
            this.BroadcastMessage("EnableSelection");
        });
        keywords.Add("Move city", () =>
        {
            // Call the Enable Selection method on every descendant object.
            this.BroadcastMessage("OnSelect");
        });
        keywords.Add("Place city", () =>
        {
            // Call the Enable Selection method on every descendant object.
            this.BroadcastMessage("OnSelect");
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray(), ConfidenceLevel.Low);

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    /// <summary>
    /// Invokes the action assosiated with the keyword said
    /// </summary>
    /// <param name="args"></param>
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {

        Debug.Log("Keyword: " + args.text + "; Confidence: " + args.confidence);
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}