using DG.Tweening;
using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    [SerializeField][Min(1)] int _startHP;
    [SerializeField]
    [Tooltip("How long, in seconds, the entity will not take damage after being hit")]float _iframeLength;

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
        if (_currentHP == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (_attachedEntity as BossController != null) //it's the boss that died, display victory
        {

        }
        else if (_attachedEntity as PlayerController != null) //skill issue from the player, display lose
        {

        }
    }
}
