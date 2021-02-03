using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostIt : MonoBehaviour
{
    [Header("Post-It Properties")]
    public string text;
    public Sword parentSword;

    private readonly float stabbingAngle = 100f;

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
                    if (parentSword != null && parentSword != stabbingSword)
                    {
                        parentSword.postIts.Remove(gameObject);
                        parentSword.postIts.TrimExcess();
                        parentSword.ArrangeInCircle();

                    }

                    parentSword = stabbingSword;
                    parentSword.postIts.Add(gameObject);

                    transform.parent = parentSword.postItsParent.transform;
                    parentSword.ArrangeInCircle();

                }
            }
        }
    }
}
