using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueController : MonoBehaviour
{
    [SerializeField] private GameObject key;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("hasKey") == 0 && PlayerPrefs.GetInt("IsFree") == 1 || PlayerPrefs.GetInt("hasKey") == 1 && PlayerPrefs.GetInt("IsFree") == 0)
        {
            Destroy(key);
        }
    }
}
