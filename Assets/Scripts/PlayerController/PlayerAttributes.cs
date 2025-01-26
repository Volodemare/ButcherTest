using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private SoundData Sounds;
    
    [SerializeField] private List<SkinnedMeshRenderer> _models = new List<SkinnedMeshRenderer>();
    [SerializeField] private SkinnedMeshRenderer playermodel;
    [SerializeField] private List<VisualEffect> _effects = new List<VisualEffect>();
    [SerializeField] private PickupTextManager textManager;

    private int newModelId;
    private float prevA = 25f;
    private int currentModel = 1;
    private ModelSwitcher switcher;

    private void Start()
    {
        GameManager.Default.OnProgressChanged.AddListener(ChangeModel);
        GameManager.Default.OnProgressChanged.AddListener(MoneyEffect);
        GameManager.Default.OnMoneyChanged.AddListener(MoneyVfx);
        switcher = transform.Find("Girl").GetComponent<ModelSwitcher>();
        switcher.SwitchModel(currentModel);
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
        newModelId = Mathf.FloorToInt(progressValue * 5);
        if (currentModel != newModelId && newModelId < 5)
        {
            switcher.SwitchModel(newModelId);
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
            
            switcher.CurrentModel.GetComponent<Animator>().SetBool("Started", true);
            switcher.CurrentModel.GetComponent<Animator>().SetTrigger("Upgraded");
        }
        
    }
}
