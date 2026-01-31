using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HealthModule : MonoBehaviour
{
    [System.Serializable]
    class PercentageToCallback
    {
        [Range(0, 100)] public int Percentage;
        public UnityEvent Callback;
    }

    [SerializeField][Min(1)] int _startHP;
    [SerializeField]
    [Tooltip("How long, in seconds, the entity will not take damage after being hit")]float _iframeLength;

    [Space]

    [SerializeField] List<PercentageToCallback> _hpEvents;

    Entity _attachedEntity;
    int _currentHP;
    bool _isInIframes;

    void Start()
    {
        _attachedEntity = GetComponent<Entity>();
        _currentHP = _startHP;
    }

    public void TakeDamage(int amount)
    {
        if (_isInIframes)
            return;

        _isInIframes = true;
        DOVirtual.DelayedCall(_iframeLength, () => _isInIframes = false, false);
        _currentHP = System.Math.Max(_currentHP - amount, 0);
        float healthPercentage = (float)_currentHP / _startHP;
        ActivatePercentageBasedEffect(healthPercentage);

        if (_attachedEntity as PlayerController != null)
        {
            EventBus.Instance.OnPlayerHealthChange?.Invoke(healthPercentage);
        }
        else if (_attachedEntity as BossController != null)
        {
            EventBus.Instance.OnBossHealthChange?.Invoke(healthPercentage);
        }

        if (_currentHP == 0)
        {
            EndGame();
        }
        else
        {
            if (_attachedEntity is BossController)
                _attachedEntity.transform.DOShakePosition(0.2f, Vector2.right * 0.5f, 35);
            else if (_attachedEntity is PlayerController)
            {
                (_attachedEntity as PlayerController).PlayDamageSFX();
            }
        }
    }

    private void EndGame()
    {
        if (_attachedEntity as BossController != null) //it's the boss that died, display victory
        {
            EventBus.Instance.OnBossDefeat?.Invoke();
        }
        else if (_attachedEntity as PlayerController != null) //skill issue from the player, display lose
        {
            EventBus.Instance.OnPlayerDeath?.Invoke();
        }
        enabled = false;
    }

    void ActivatePercentageBasedEffect(float percentage)
    {
        PercentageToCallback effect = _hpEvents.FirstOrDefault(e => e.Percentage / 100f >= percentage);
        if (effect != null)
        {
            effect.Callback.Invoke();
            _hpEvents.Remove(effect);
        }
    }
}
