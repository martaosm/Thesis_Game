using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleController : MonoBehaviour
{
    [SerializeField] private GameObject demonGuard;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("DemonGuardDone") == 1 && PlayerPrefs.GetInt("IsGuardAnEnemy") == 0)
        {
            demonGuard.SetActive(true);
        }
    }
}
