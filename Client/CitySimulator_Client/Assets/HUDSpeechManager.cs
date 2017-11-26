using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Module: HUDSpeechManager
/// Team: Hololens
/// Description: Handler for speech commands for the user to use for the menu in the main scene.
/// Author:
///	 Name: George Lee   Date: 2017-11-26
///  Name: Steven Ma			Date: 2017-11-26
/// Based on:
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class HUDSpeechManager : MonoBehaviour {

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("Quit game", () =>
        {
            // Call the OnReset method on every descendant object.
            transform.Find("Quit_Popup").gameObject.SetActive(true);
        });

        keywords.Add("Yes", () =>
        {
            if (!transform.Find("Quit_Popup").gameObject.activeInHierarchy)
            {
                return;
            }
            this.BroadcastMessage("backToMenu");
        });

        keywords.Add("No", () =>
        {
            if (!transform.Find("Quit_Popup").gameObject.activeInHierarchy)
            {
                return;
            }
            transform.Find("Quit_Popup").gameObject.SetActive(false);
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
