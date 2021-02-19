using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpawner : MonoBehaviour
{
    [Header("Swords")]
    public Sword[] swords;

    [Header("Effects")]
    public GameObject fireCrackle;

    public GameObject GroupHolder1;
    public GameObject GroupHolder2;

    private int currentSword = 0;

    public static readonly Vector3 swordInitialRotation = new Vector3(180f, 90f, 20f);
    public static readonly Vector3 swordInitialPosition = new Vector3(-0.072f, 0.96f, 0.195f);

    // Start is called before the first frame update
    void Start()
    {
        fireCrackle.SetActive(false);

        AllignSwords();

        Sword.onSwordFirstPickUp += GenerateNewSword;
    }

    private void AllignSwords()
    {
        for (int i = 1; i < swords.Length; i++)
        {
            Debug.Log($"Current sword: {i}");
            swords[i].transform.position = swordInitialPosition;
            swords[i].transform.rotation = Quaternion.Euler(swordInitialRotation);
        }
    }

    private void GenerateNewSword()
    {
        currentSword++;
        Debug.Log($"Current sword: {currentSword}");
        
        
        StartCoroutine(PlayEffect());

        if(currentSword == 5)
        {
            GroupHolder1.GetComponent<GroupHolderManager>().ShowTop();
        }
        if(currentSword == 7)
        {
            GroupHolder2.GetComponent<GroupHolderManager>().ShowTop();
        }

    }

    private IEnumerator PlayEffect()
    {
        fireCrackle.SetActive(true);

        yield return new WaitForSeconds(2f);

        fireCrackle.SetActive(false);

        Sword newSword = swords[currentSword];
        newSword.gameObject.SetActive(true);
    }
}
