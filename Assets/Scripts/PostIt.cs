using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostIt : MonoBehaviour
{
    public string text;

    private readonly float stabbingAngle = 140f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Sword stabbingSword = other.GetComponent<Sword>();
            Debug.Log("Stabbing angle: " + Vector3.Angle(transform.forward, other.gameObject.transform.up));
            if (Vector3.Angle(transform.forward, other.gameObject.transform.up) > stabbingAngle)
            {
                Debug.Log("Stabbed");
                if (!stabbingSword.postIts.Contains(gameObject))
                {
                    stabbingSword.postIts.Add(gameObject);

                    transform.parent = stabbingSword.postItParent.transform;
                    Debug.Log("Added");
                }
            }
        }
    }
}
