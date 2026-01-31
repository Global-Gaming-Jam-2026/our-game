using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] SpriteRenderer _hpBarImage;
    [SerializeField] Sprite[] _hpSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnBossHealthChange += UpdateBossHP;
    }

    void UpdateBossHP(float percentage)
    {
        int index = 0;
        
        if (percentage <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (percentage <= 0.1f)
        {
            index = 4;
        }
        else if (percentage <= 0.2f)
        {
            index = 3;
        }
        else if (percentage <= 0.4f)
        {
            index = 2;
        }
        else if (percentage <= 0.6f)
        {
            index = 1;
        }
        else if (percentage <= 0.8f)
        {
            index = 0;
        }

        _hpBarImage.sprite = _hpSprites[index];
    }
}
