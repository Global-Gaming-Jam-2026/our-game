using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    [Header("Win")]
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private TMP_Text winText;

    [Header("Lose")]
    [SerializeField] private CanvasGroup losePanel;
    [SerializeField] private TMP_Text loseText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;

    private void Start()
    {
        // Hide panels at start
        if (winPanel != null) winPanel.gameObject.SetActive(false);
        if (losePanel != null) losePanel.gameObject.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        // 1. Subscribe to the EventBus instead of GameManager
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnPlayerDeath += ShowLoseScreen;
            EventBus.Instance.OnBossDefeat += ShowWinScreen;
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
        Debug.Log("Restart button was physically pressed!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager Instance is NULL! Did you forget to put a GameManager in the scene?");
        }
    }

    private void OnDestroy()
    {
        // 3. Unsubscribe to prevent memory leaks/errors
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnPlayerDeath -= ShowLoseScreen;
            EventBus.Instance.OnBossDefeat -= ShowWinScreen;
        }
    }
}
