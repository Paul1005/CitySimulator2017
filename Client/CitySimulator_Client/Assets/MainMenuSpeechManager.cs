using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Module: MainMenuSpeechManager
/// Team: Hololens
/// Description: Handles speech commands for user to select options from the main menu.
/// Author:
///	 Name: Steven Ma   Date: 2017-11-26
///  Name: George Lee			Date: 2017-11-26
/// Based on:
/// https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_212
/// </summary>
public class MainMenuSpeechManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer = null;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    private void Start()
    {
        keywords.Add("Play", () =>
        {
            Debug.LogWarning("Voice play");
            this.SendMessage("playGameBtn");
            this.BroadcastMessage("playGameBtn");
        });

        keywords.Add("Exit", () =>
        {
            Debug.LogWarning("Voice exit");
            this.BroadcastMessage("exitGameBtn");
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