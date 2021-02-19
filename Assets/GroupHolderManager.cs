using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupHolderManager : MonoBehaviour
{
    public GameObject TopPart;

    private void Start()
    {
        TopPart.SetActive(false);
    }
    internal void ShowTop()
    {
        TopPart.SetActive(true);
    }
}
