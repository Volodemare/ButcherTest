using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private SoundData Sounds;
    
    [SerializeField] private List<Mesh> _models = new List<Mesh>();
    [SerializeField] private MeshFilter playermodel;
    [SerializeField] private List<VisualEffect> _effects = new List<VisualEffect>();
    [SerializeField] private PickupTextManager textManager;

    private int newModelId;
    private float prevA = 25f;
    private int currentModel = 1;

    private void Start()
    {
        GameManager.Default.OnProgressChanged.AddListener(ChangeModel);
        GameManager.Default.OnProgressChanged.AddListener(MoneyEffect);
        GameManager.Default.OnMoneyChanged.AddListener(MoneyVfx);
        playermodel.mesh = _models[currentModel];
    }

    private void MoneyVfx(int amount)
    {
        textManager.ShowPickup(GameManager.Default.oldMoney);
    }
    public void MoneyEffect(float a)
    {
        
        if (a >= prevA)
        {
            Sounds.PlaySound(0);
            _effects[0].Play();
        }
        else
        {
            Sounds.PlaySound(2);
            _effects[1].Play();
        }

        prevA = a;
    }

    public void ChangeModel(float progressValue)
    {
        newModelId = Mathf.FloorToInt(progressValue * _models.Count);
        if (currentModel != newModelId && newModelId < _models.Count)
        {
            transform.Find("Girl").GetComponent<Animator>().SetTrigger("Upgrade");
            playermodel.mesh = _models[newModelId];
            if (newModelId > currentModel)
            {
                _effects[2].Play();
                Sounds.PlaySound(1);
            }
            else
            {
                _effects[3].Play();
                Sounds.PlaySound(3);
            }
            
            currentModel = newModelId;
            
        }
        
    }
}
