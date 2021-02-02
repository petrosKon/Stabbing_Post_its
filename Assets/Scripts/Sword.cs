using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : MonoBehaviour
{
    public string swordName;
    public Transform finalTransform;
    public List<GameObject> postIts = new List<GameObject>();
    public GameObject postItParent;
    public List<Transform> postItsFinalTransforms = new List<Transform>();

    private readonly Vector3 swordFinalRotation = new Vector3(180, 90, 180);
    private readonly Vector3 postItFinalRotation = new Vector3(0, -90, 180);
    public void ReturnToFinalPosition()
    {
        transform.position = finalTransform.position;
        transform.rotation = Quaternion.Euler(swordFinalRotation);
    }

    public void ArrangePostIts()
    {
        if(postIts.Count != 0)
        {
            for (int i = 0; i < postIts.Count; i++)
            {
                postIts[i].transform.position = postItsFinalTransforms[i].transform.position;
                postIts[i].transform.rotation = Quaternion.Euler(postItFinalRotation);
            }
        }
        
    }

    private void Update()
    {

    }
}
