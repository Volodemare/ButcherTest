// GameManager.cs

using ButchersGames;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Default;

    // События
    public UnityEvent<float> OnProgressChanged; // float: 0-1
    public UnityEvent<int> OnMoneyChanged;
    public UnityEvent<int> OnTotalMoneyChanged;

    private int _totalMoney;
    [SerializeField] private int _currentMoney;
    [SerializeField] private TMP_Text levelText;
    private float _currentProgress = 25f;
    private float _maxProgress = 99f;

    public int TotalMoney {get{return _totalMoney;}}
    public float CurrentProgress {get{return _currentProgress;}}
    public int CurrentMoney 
    {
        get
        {
            return _currentMoney;
        }
        set
        {
            _currentMoney = value;
            OnMoneyChanged?.Invoke(_currentMoney);
        }
        
    }

    private void Awake()
    {
        if (Default == null)
        {
            Default = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
            OnTotalMoneyChanged?.Invoke(_totalMoney);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void AddMoney(int amount)
    {
        _currentMoney = Mathf.Clamp(_currentMoney + amount, 0, int.MaxValue);
        OnMoneyChanged?.Invoke(_currentMoney);
    }

    public void UpdateProgress(float amount)
    {
        _currentProgress = Mathf.Clamp(_currentProgress + amount, 0, _maxProgress);
        OnProgressChanged?.Invoke(_currentProgress / _maxProgress);
        if (_currentProgress <= 0)
        {
            LevelManager.Default.RestartLevel();
        }
    }

    public void FinishLevel()
    {
        _totalMoney += _currentMoney;
        _currentMoney = 0;
        _currentProgress = 25f;
        SaveData();
        OnTotalMoneyChanged?.Invoke(_totalMoney);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("TotalMoney", _totalMoney);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        _totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
    }
}