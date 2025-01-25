using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private float neededProgress = 20f;
    [SerializeField] private int multiplier = 5;
    [SerializeField] private int soundId = 0;
    [SerializeField] private SoundData soundData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Default.CurrentProgress >= neededProgress)
            {
                GetComponent<Animator>().SetTrigger("Open");
                GameManager.Default.CurrentMoney = GameManager.Default.CurrentMoney * multiplier;
                soundData.PlaySound(soundId);
            }
            else
            {
                other.GetComponent<PlayerController>().StopGame();
                GameObject.Find("UIManager").GetComponent<UIManager>().CallEndUI();

            }
        }
    }
}
