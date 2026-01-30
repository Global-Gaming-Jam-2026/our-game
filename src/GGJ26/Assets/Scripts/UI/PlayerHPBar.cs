using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] Image _hpBarFill;

    [SerializeField] RectTransform _hpBarRectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnPlayerHealthChange += UpdateHPBar;
    }

    void UpdateHPBar(float percentage)
    {
        _hpBarRectTransform.DOShakePosition(0.3f, 20f, 30);
        _hpBarFill.fillAmount = percentage;
    }
}
