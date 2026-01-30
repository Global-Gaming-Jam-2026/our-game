using UnityEngine;

/// <summary>
/// Bridges EventBus death events to GameManager state changes.
/// Add this to a GameObject in the gameplay scene.
/// </summary>
public class GameStateListener : MonoBehaviour
{
    private void Start()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnPlayerDeath += HandlePlayerDeath;
            EventBus.Instance.OnBossDefeat += HandleBossDefeat;
        }
        else
        {
            Debug.LogWarning("GameStateListener: EventBus.Instance is null");
        }
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnPlayerDeath -= HandlePlayerDeath;
            EventBus.Instance.OnBossDefeat -= HandleBossDefeat;
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died - triggering game over");
        GameManager.Instance?.PlayerLost();
    }

    private void HandleBossDefeat()
    {
        Debug.Log("Boss defeated - triggering victory");
        GameManager.Instance?.PlayerWon();
    }
}
