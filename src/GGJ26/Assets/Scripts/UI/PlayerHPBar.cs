using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] RectTransform _hpBarRectTransform;
    [SerializeField] Image _fullHPImage;
    [SerializeField] Image[] _hpImages;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnPlayerHealthChange += UpdateHPBar;
    }

    void UpdateHPBar(float percentage)
    {
        _hpBarRectTransform.DOShakePosition(0.3f, 20f, 30);

        //0.8 = 0
        //0.6 = 1
        //0.4 = 2
        //0.2 = 3
        //0 = 4
        int index = Mathf.RoundToInt(4 - percentage * 5);
        UpdateHPImages(index);
    }

    void UpdateHPImages(int indexToEnable)
    {
        _fullHPImage.gameObject.SetActive(false);
        foreach (Image image in _hpImages)
        {
            image.gameObject.SetActive(false);
        }
        _hpImages[indexToEnable].gameObject.SetActive(true);
    }
}
