using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : MonoBehaviour
{
    public string swordName;
    public Transform finalTransform;
    public List<GameObject> postIts;

    public void ReturnToInitialPosition()
    {
        transform.position = finalTransform.position;
    }

    private void Update()
    {
        
    }
}
