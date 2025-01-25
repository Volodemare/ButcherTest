using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private int RightRotateNorm = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is rotating");
            other.transform.Rotate(new Vector3(0, RightRotateNorm, 0), 90);
        }
    }
}
