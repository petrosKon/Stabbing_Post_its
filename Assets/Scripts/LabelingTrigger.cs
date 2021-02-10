using System;
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LabelingTrigger : MonoBehaviour
{
    public Text canvasText; 

    private Sword activeSword;
    private LabelState state;
    private AudioSource audio;
    private SpeechRecognition speechRecognition;

    private void Start()
    {
        activeSword = null;
        state = LabelState.Idle;
        audio = GetComponent<AudioSource>();
        speechRecognition = FindObjectOfType<SpeechRecognition>();
        speechRecognition.OnSpeechRecognized += NameSword;
    }

    private void NameSword(string name)
    {
        // TODO: Voice that tells that sword was named
        Debug.Log($"Sword named {name}");
        activeSword.swordName = name;
        activeSword.StopLabeling();

        state = LabelState.Idle;
        activeSword = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state == LabelState.Active)
        {
            // Debug.Log("LabelState already active");
            return;
        }

        state = LabelState.Active;
        activeSword = GetSword(other);


        if (activeSword == null)
        {
            // Debug.Log("No Sword found");
            return;
        }

        StartCoroutine(VoiceFromTheSkies());
    }

    private IEnumerator VoiceFromTheSkies()
    {
        activeSword.VibrateController();
        audio.Play();

        yield return new WaitForSeconds(1);

        activeSword.StartLabeling();

        yield return new WaitForSeconds(audio.clip.length - 1);

        speechRecognition.startRecording();

        yield return new WaitForSeconds(5);

        speechRecognition.stopRecording();


        yield return new WaitForSeconds(4);

        if (state == LabelState.Active) // If no OnSpeechRecognized event came, just deactivate labeling 
        {
            Debug.Log("No SpeechRecognised after 4 seconds");
            activeSword.StopLabeling();
            state = LabelState.Idle;
            activeSword = null;

            // TODO: Voice that tells that no name was heard
        }
    }


    private Sword GetSword(Collider collider)
    {
        if (collider.GetComponent<Collider>().GetType() == typeof(BoxCollider))
        {
            if (collider.CompareTag("Sword"))
            {
                return collider.GetComponent<Sword>();
            }
        }

        return null;
    }
}

public enum LabelState
{
    Idle,
    Active
}
