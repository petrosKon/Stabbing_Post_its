﻿using Autohand;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sword : MonoBehaviour
{
    [Header("Sword Properties")]
    public string swordName;
    public Text swordNamePreview;
    public Text swordNameLabel;
    public Transform finalTransform;
    public List<GameObject> postIts = new List<GameObject>();
    public GameObject postItsParent;
    public List<Transform> postItsFinalTransforms = new List<Transform>();
    public Hand grabbedBy;
    public bool firstPicked = false;
    
    public GameObject LaserStyle;
    private GameObject LaserInstance;
    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;
    private AudioSource audio;

    internal bool HasSpace()
    {
        return postIts.Count < 10;
    }

    private bool namePreview;

    public static readonly Vector3 swordFinalRotation = new Vector3(180, 90, 180);
    public static readonly Vector3 postItFinalRotation = new Vector3(0, -90, 0);
    public static readonly Vector3 postItCircularRotation = new Vector3(0, 0, 0);
    public static readonly float vibrationSeconds = 0.5f;

    public static readonly float rotationAmount = 200f;
    public static readonly float grabDistance = 0.1f;

    private GameObject leftHand;
    private GameObject rightHand;

    public static Action onSwordFirstPickUp;

    private GameObject displayWall;

    public enum Hand
    {
        None,
        Left,
        Right
    }

    private void Start()
    {
        leftHand = GameObject.FindGameObjectWithTag("LeftHand");
        rightHand = GameObject.FindGameObjectWithTag("RightHand");

        displayWall = finalTransform.gameObject.transform.parent.gameObject;
        audio = GetComponent<AudioSource>();
        namePreview = false;
        audio.loop = true;
        swordNamePreview.text = swordName;
    }

    public void brieflyShowSwordName()
    {
        swordNamePreview.text = swordName;
        swordNameLabel.text = swordName;
        StartCoroutine(ShowSwordText());
    }

    private IEnumerator ShowSwordText()
    {
        namePreview = true;
        yield return new WaitForSeconds(5);
        namePreview = false;
    }

    public void ReturnToFinalPosition()
    {
        StartCoroutine(ReturnToFinalPositionCoroutine());
    }

    public IEnumerator ReturnToFinalPositionCoroutine()
    {
        transform.position = finalTransform.position;
        transform.rotation = finalTransform.rotation;
        //transform.rotation = Quaternion.Euler(new Vector3(swordFinalRotation.x, swordFinalRotation.y + displayWall.transform.rotation.eulerAngles.y, swordFinalRotation.z));

        yield return new WaitForSeconds(vibrationSeconds);

        grabbedBy = Hand.None;
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

    public void VibrateController()
    {
        if (grabbedBy == Hand.Right)
        {
            StartCoroutine(Vibrate(0.5f, OVRInput.Controller.RTouch));
        }
        else if (grabbedBy == Hand.Left)
        {
            StartCoroutine(Vibrate(0.5f, OVRInput.Controller.LTouch));
        }
    }

    IEnumerator Vibrate(float Seconds, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(1, 1, controller);

        yield return new WaitForSeconds(Seconds);

        OVRInput.SetControllerVibration(0, 0, controller);

    }


    public void StartLabeling() {
        OVRInput.Controller controller = grabbedBy == Hand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
        OVRInput.SetControllerVibration(1, 1, controller);
        
        Destroy(LaserInstance);

        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x -90, rot.y, rot.z);

        LaserInstance = Instantiate(LaserStyle, postItsParent.transform.position, Quaternion.Euler(rot));
        LaserInstance.transform.parent = transform;
        LaserScript = LaserInstance.GetComponent<Hovl_Laser>();
        LaserScript2 = LaserInstance.GetComponent<Hovl_Laser2>();

        audio.Play();
    }

    public void StopLabeling()
    {
        audio.Stop();
        OVRInput.Controller controller = grabbedBy == Hand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
        OVRInput.SetControllerVibration(0, 0, controller);

        if (LaserScript) LaserScript.DisablePrepare();
        if (LaserScript2) LaserScript2.DisablePrepare();
        Destroy(LaserInstance, 1);
    }



    public void ArrangePostIts()
    {
        if (postIts.Count != 0)
        {
            for (int i = 0; i < postIts.Count; i++)
            {
                postIts[i].transform.position = postItsFinalTransforms[i].transform.position;
                postIts[i].transform.rotation = postItsFinalTransforms[i].transform.rotation;
                //postIts[i].transform.rotation = Quaternion.Euler(new Vector3(postItFinalRotation.x, postItFinalRotation.y + displayWall.transform.rotation.eulerAngles.y, postItFinalRotation.z));
            }
        }
    }

    private void Update()
    {
        swordNamePreview.gameObject.SetActive(namePreview);
        if (Vector3.Distance(transform.position, rightHand.transform.position) < grabDistance && transform.parent.tag.Equals("Player"))
        {
            PickAndRotateSword(Hand.Right, OVRInput.Axis2D.SecondaryThumbstick);
        }
        else if (Vector3.Distance(transform.position, leftHand.transform.position) < grabDistance && transform.parent.tag.Equals("Player"))
        {
            PickAndRotateSword(Hand.Left, OVRInput.Axis2D.PrimaryThumbstick);
        }

    }

    public bool IsInFinalPosition()
    {
        return transform.position == finalTransform.position && transform.rotation == finalTransform.rotation;
    }

    private void PickAndRotateSword(Hand pickedBy, OVRInput.Axis2D thumbstick)
    {
        if (!firstPicked)
        {
            firstPicked = true;
            onSwordFirstPickUp();
        }

        grabbedBy = pickedBy;
        if (OVRInput.Get(thumbstick).x > 0.5f)
        {
            postItsParent.transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime);
        }
        else if (OVRInput.Get(thumbstick).x < -0.5f)
        {
            postItsParent.transform.Rotate(Vector3.up, -rotationAmount * Time.deltaTime);

        }
    }
}
