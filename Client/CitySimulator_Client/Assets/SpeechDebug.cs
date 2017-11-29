using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechDebug : MonoBehaviour
{

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
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
                print("Dictation completed unsuccessfully: " + completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            print("Dictation error: {0}; HResult = {1}." + error + hresult);
        };

        m_DictationRecognizer.Start();
    }
}