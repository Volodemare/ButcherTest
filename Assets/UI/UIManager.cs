using ButchersGames;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text totalMoneyText;
    [SerializeField] private TMP_Text currentMoneyText;
    [SerializeField] private Button _button;

    [SerializeField] private TMP_Text endButton;
    [SerializeField] private GameObject endPanel;
    //[SerializeField] private GameObject lowProgressWarning;
    private void Awake()
    {
        UpdateTotalMoney(GameManager.Default.TotalMoney);
    }
    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Default.OnProgressChanged.AddListener(UpdateProgressBar);
        GameManager.Default.OnMoneyChanged.AddListener(UpdateCurrentMoney);
        GameManager.Default.OnTotalMoneyChanged.AddListener(UpdateTotalMoney);
        _button.onClick.AddListener(EndLevelButton);
    }

    private void UnsubscribeFromEvents()
    {
        GameManager.Default.OnProgressChanged.RemoveListener(UpdateProgressBar);
        GameManager.Default.OnMoneyChanged.RemoveListener(UpdateCurrentMoney);
        GameManager.Default.OnTotalMoneyChanged.RemoveListener(UpdateTotalMoney);
    }

    private void UpdateProgressBar(float progress)
    {
        progressSlider.DOValue(progress, 0.4f).SetEase(Ease.OutCubic);
    }
    private void UpdateCurrentMoney(int amount)
    {
        currentMoneyText.text = $"{amount}";
        endButton.text = $"    {amount}   <sprite index=0>";
    }
    private void UpdateTotalMoney(int amount)
    {
        totalMoneyText.text = $"{amount}";
    }

    public void CallEndUI()
    {
        endPanel.SetActive(true);
        endPanel.GetComponent<Animator>().SetBool("Call", true);
    }
    
    private void EndLevelButton()
    {
        GameManager.Default.FinishLevel();
        endPanel.GetComponent<Animator>().SetBool("Call", false);
        LevelManager.Default.NextLevel();
        
    }

}