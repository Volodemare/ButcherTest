using UnityEngine;
using TMPro;
using DG.Tweening;

public class PickupTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickupText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private float autoHideDelay = 1f;

    private int _totalValue;
    private Vector3 _initialPosition;
    private Sequence _animationSequence;
    private float _lastPickupTime;

    private void Start()
    {
        _initialPosition = pickupText.rectTransform.anchoredPosition;
        pickupText.alpha = 0f;
        pickupText.gameObject.SetActive(false);
    }

    public void ShowPickup(int value)
    {
        if (value < 0)
        {
            if (_totalValue > 0)
            {
                _totalValue = 0;
            }
            pickupText.color = Color.red;
        }
        else
        {
            if (_totalValue < 0)
            {
                _totalValue = 0;
            }
            pickupText.color = Color.green;
        }
        _totalValue += value;
        UpdateText();
        ResetAnimation();
        RestartAutoHideTimer();
    }

    private void UpdateText()
    {
        pickupText.text = $"+ {_totalValue} $";
    }

    private void ResetAnimation()
    {
        _animationSequence?.Kill();

        pickupText.gameObject.SetActive(true);
        pickupText.rectTransform.anchoredPosition = _initialPosition;
        pickupText.alpha = 1f;

        _animationSequence = DOTween.Sequence()
            .Append(pickupText.rectTransform.DOAnchorPosY(
                _initialPosition.y + moveDistance, 
                fadeDuration))
            .Join(pickupText.DOFade(0f, fadeDuration))
            .OnComplete(() => 
            {
                if (Time.time - _lastPickupTime >= autoHideDelay)
                {
                    pickupText.gameObject.SetActive(false);
                    _totalValue = 0;
                }
            });
    }

    private void RestartAutoHideTimer()
    {
        _lastPickupTime = Time.time;
    }
}