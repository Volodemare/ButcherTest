using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    [Header("Модели для замены")]
    public GameObject[] skeletonModels; 
    private GameObject currentModel;  
    public GameObject CurrentModel => currentModel;

    void Start()
    {
        SwitchModel(0);
    }
    
    public void SwitchModel(int modelIndex)
    {
        if (modelIndex < 0 || modelIndex >= skeletonModels.Length)
        {
            Debug.LogError("Неверный индекс модели!");
            return;
        }

        // Удаляем текущую модель
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Создаем новую модель
        currentModel = Instantiate(skeletonModels[modelIndex], transform);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;
        
        Animator newAnimator = currentModel.AddComponent<Animator>();
        if (newAnimator != null)
        {
            newAnimator.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
        }
    }
}
