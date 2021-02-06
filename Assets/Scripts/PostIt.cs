using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostIt : MonoBehaviour
{
    [Header("Post-It Properties")]
    public string text;
    public Sword parentSword;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().GetType() == typeof(BoxCollider))
        {
            if (other.CompareTag("Sword"))
            {
                Sword stabbingSword = other.GetComponent<Sword>();
                Debug.Log("Stabbing angle: " + Vector3.Angle(transform.forward, other.gameObject.transform.up));

                if (!stabbingSword.postIts.Contains(gameObject))
                {
                    stabbingSword.VibrateController();

                    if (parentSword != null && parentSword != stabbingSword)
                    {
                        parentSword.postIts.Remove(gameObject);
                    }

                    parentSword = stabbingSword;
                    parentSword.postIts.Add(gameObject);
                    Debug.Log("GameObject name: " + gameObject.name + " Parent: " + gameObject.transform.parent);

                    transform.parent = parentSword.postItsParent.transform;
                    parentSword.ArrangeInCircle();
                }
            }
        }
    }
}
