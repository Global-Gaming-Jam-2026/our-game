using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void Update()
    {
        // Only check for ESC when game is playing or paused
        if (GameManager.Instance == null) return;

        var state = GameManager.Instance.CurrentState;
        if (state != GameManager.GameState.Playing && state != GameManager.GameState.Paused)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.PauseGame();
            ShowPauseMenu();
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
        {
            GameManager.Instance.ResumeGame();
            HidePauseMenu();
        }
    }

    private void ShowPauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    private void HidePauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void OnResumeClicked()
    {
        GameManager.Instance?.ResumeGame();
        HidePauseMenu();
    }

    private void OnQuitClicked()
    {
        GameManager.Instance?.QuitToMenu();
    }
}
