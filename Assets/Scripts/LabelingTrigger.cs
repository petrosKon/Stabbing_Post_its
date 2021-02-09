using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelingTrigger : MonoBehaviour
{
    private Sword activeSword;
    private LabelState state;

    private void Start()
    {
        activeSword = null;
        state = LabelState.Idle;
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

        activeSword.StartVibration();
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
        activeSword.EndVibration();
        activeSword = null;
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
