using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechDebug : MonoBehaviour
{
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private DictationRecognizer m_DictationRecognizer;

    void Start()
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

        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            print("Dictation result: " + text + " " + confidence);
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            print("Dictation hypothesis: " + text);
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
            {
                print("Dictation completed unsuccessfully: " + completionCause);
            }
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            print("Dictation error: {0}; HResult = {1}." + error + hresult);
        };
        m_DictationRecognizer.Start();
    }
}