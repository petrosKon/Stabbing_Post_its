using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : MonoBehaviour
{
    [Header("Sword Properties")]
    public string swordName;
    public Transform finalTransform;
    public List<GameObject> postIts = new List<GameObject>();
    public GameObject postItsParent;
    public List<Transform> postItsFinalTransforms = new List<Transform>();
    public GameObject rightHand;
    public GameObject leftHand;
    public Hand grabbedBy;

    public static readonly Vector3 swordFinalRotation = new Vector3(180, 90, 180);
    public static readonly Vector3 postItFinalRotation = new Vector3(0, -90, 180);
    public static readonly Vector3 postItCircularRotation = new Vector3(0, 0, 0);
    public static readonly float rotationAmount = 200f;
    public static readonly float grabDistance = 0.07f;

    public enum Hand
    {
        Left,
        Right
    }

    public void ReturnToFinalPosition()
    {
        transform.position = finalTransform.position;
        transform.rotation = Quaternion.Euler(swordFinalRotation);
    }

    public void ArrangeInCircle()
    {
        float radius = 0.3f;
        for (int i = 0; i < postIts.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / postIts.Count;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, postItsParent.transform.localPosition.y, Mathf.Sin(angle) * radius);
            postIts[i].transform.localPosition = newPos;
            postIts[i].transform.localRotation = Quaternion.Euler(postItCircularRotation);
        }
    }

    public void ArrangePostIts()
    {
        if (postIts.Count != 0)
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
        if(Vector3.Distance(transform.position, rightHand.transform.position) < grabDistance)
        {
            grabbedBy = Hand.Right;
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x > 0.5f)
            {
                postItsParent.transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime);
            }
            else if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x < -0.5f)
            {
                postItsParent.transform.Rotate(Vector3.up, -rotationAmount * Time.deltaTime);

            }
        } else if(Vector3.Distance(transform.position, leftHand.transform.position) < grabDistance)
        {
            grabbedBy = Hand.Left;
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.5f)
            {
                postItsParent.transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime);
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.5f)
            {
                postItsParent.transform.Rotate(Vector3.up, -rotationAmount * Time.deltaTime);

            }
        }
    }
}
