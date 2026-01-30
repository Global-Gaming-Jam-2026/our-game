using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] Image _hpBarImage;
    [SerializeField] RectTransform _hpBarRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnBossHealthChange += UpdateBossHP;
    }

    void UpdateBossHP(float percentage)
    {
        _hpBarRect.DOShakePosition(0.3f, 20f, 30);
        _hpBarImage.fillAmount = percentage;
    }
}
