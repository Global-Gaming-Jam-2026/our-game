using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

/// <summary>
/// Main menu controller with Play and Quit functionality.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup menuPanel;
    [SerializeField] private TMP_Text titleText;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Settings")]
    [SerializeField] private string gameSceneName = "Almog Prototype";

    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        AnimateIn();
    }

    private void AnimateIn()
    {
        if (menuPanel != null)
        {
            menuPanel.alpha = 0f;
            menuPanel.DOFade(1f, 0.5f).SetUpdate(true);
        }

        if (titleText != null)
        {
            titleText.transform.localScale = Vector3.zero;
            titleText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }

    private void OnPlayClicked()
    {
        if (menuPanel != null)
        {
            menuPanel.DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() => {
                SceneManager.LoadScene(gameSceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit requested");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
