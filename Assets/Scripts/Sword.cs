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


    public GameObject LaserStyle;
    private GameObject LaserInstance;
    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;


    public static readonly Vector3 swordFinalRotation = new Vector3(180, 90, 180);
    public static readonly Vector3 postItFinalRotation = new Vector3(0, -90, 0);
    public static readonly Vector3 postItCircularRotation = new Vector3(0, 0, 0);
    public static readonly float rotationAmount = 200f;
    public static readonly float grabDistance = 0.07f;

    private GameObject displayWall;

    public enum Hand
    {
        Left,
        Right
    }

    private void Start()
    {
        displayWall = finalTransform.gameObject.transform.parent.gameObject;
    }

    public void ReturnToFinalPosition()
    {
        transform.position = finalTransform.position;
        transform.rotation = Quaternion.Euler(new Vector3(swordFinalRotation.x, swordFinalRotation.y + displayWall.transform.rotation.eulerAngles.y, swordFinalRotation.z));
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
        else
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
    }

    public void StopLabeling()
    {
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
                postIts[i].transform.rotation = Quaternion.Euler(new Vector3(postItFinalRotation.x, postItFinalRotation.y + displayWall.transform.rotation.eulerAngles.y, postItFinalRotation.z));
            }
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, rightHand.transform.position) < grabDistance)
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
        }
        else if (Vector3.Distance(transform.position, leftHand.transform.position) < grabDistance)
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
