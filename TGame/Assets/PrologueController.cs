using System;
using System.Collections;
using System.Collections.Generic;
using NPC.Enemies;
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
        if (PlayerPrefs.GetInt("hasMark") == 1)
        {
            var skeletons = FindObjectsOfType<EnemyController>();
            foreach(var skeleton in skeletons)
            {
                skeleton.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}
