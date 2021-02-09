using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelingTrigger : MonoBehaviour
{
    public Text canvasText; 

    private Sword activeSword;
    private LabelState state;


    private void Start()
    {
        activeSword = null;
        state = LabelState.Idle;
        UpdateText();
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

        UpdateText();
        activeSword.StartLabeling();
    }


    private void OnTriggerExit(Collider other)
    {
        Sword triggerSword = GetSword(other);

        if(!GameObject.ReferenceEquals(triggerSword, activeSword))
        {
            //Debug.Log("Active Sword is not the one that caused trigger");
            return;
        }

        state = LabelState.Idle;
        activeSword.StopLabeling();
        activeSword = null;
        UpdateText();
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

    private void UpdateText()
    {
        if(state == LabelState.Idle)
        {
            canvasText.text = "";
        }
        else
        {
            canvasText.text = "Name your Sword";
        }


    }


}

public enum LabelState
{
    Idle,
    Active
}
