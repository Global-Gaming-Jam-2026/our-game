using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string gameOverSceneName = "GameOver"; 

    [Header("Win")]
    [SerializeField] private CanvasGroup winPanel;

    [Header("Lose")]
    [SerializeField] private CanvasGroup losePanel;

    private void Start()
    {
        if (winPanel != null) winPanel.gameObject.SetActive(false);
        if (losePanel != null) losePanel.gameObject.SetActive(false);

        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnPlayerDeath += HandlePlayerDeath;
            EventBus.Instance.OnBossDefeat += HandleBossDefeat;
        }
    }

    private void HandlePlayerDeath()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }

    private void HandleBossDefeat()
    {
        if (winPanel != null)
        {
            winPanel.gameObject.SetActive(true);
            winPanel.alpha = 0;
            winPanel.DOFade(1f, 0.5f);
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

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Won)
            ShowWinScreen();
        else if (state == GameManager.GameState.Lost)
            ShowLoseScreen();
    }

    private void ShowWinScreen()
    {
        if (winPanel != null)
        {
            winPanel.gameObject.SetActive(true);
            winPanel.alpha = 0;
            winPanel.DOFade(1f, 0.5f).SetUpdate(true);
        }
    }

    private void ShowLoseScreen()
    {
        if (losePanel != null)
        {
            losePanel.gameObject.SetActive(true);
            losePanel.alpha = 0;
            losePanel.DOFade(1f, 0.5f).SetUpdate(true);
        }
    }

    private void OnRestartClicked()
    {
        GameManager.Instance?.RestartGame();
    }

}
