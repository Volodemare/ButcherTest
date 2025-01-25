using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int mp;
    [SerializeField] private bool destoryOnDeath = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Default.AddMoney(2 * mp);
            GameManager.Default.UpdateProgress(3 * mp);
            if (destoryOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }
}
