using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text mic;
    [SerializeField] private Text phrase;

    private SpeechRecognition speechRecognition;

    private IEnumerator Start()
    {
        speechRecognition = FindObjectOfType<SpeechRecognition>();
        speechRecognition.OnSpeechRecognized += UpdateUI;

        // Wait until the mic is initialized before we setup the UI.
        while (speechRecognition.mic == null)
        {
            yield return null;
        }
        SetupUI();
    }

    private void SetupUI()
    {
        // mic.text = speechRecognition.mic.Name;
        // phrase.text = "";
    }

    private void UpdateUI(string str)
    {
        StartCoroutine(UpdateUIEnum(str));
    }

    private IEnumerator UpdateUIEnum(string str)
    {
        //phrase.text = $"The sowrd is now named: {str}";   
        yield return new WaitForSeconds(10);
        //phrase.text = "";
    }
}
