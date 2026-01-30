using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class IntroSequence : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup introPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text storyText;

    [Header("Settings")]
    [SerializeField] private float titleDuration = 2f;
    [SerializeField] private float storyDuration = 3f;
    [SerializeField] private string[] storyLines = new string[]
    {
        "The ancient mask awakens...",
        "Face your destiny."
    };

    private bool isPlaying;

    public System.Action OnIntroComplete;

    private void Start()
    {
        if (introPanel != null)
        {
            StartCoroutine(PlayIntro());
        }
    }

    private void Update()
    {
        if (isPlaying && Input.anyKeyDown)
        {
            SkipIntro();
        }
    }

    private IEnumerator PlayIntro()
    {
        isPlaying = true;
        Time.timeScale = 0;

        // Show title
        if (titleText != null)
        {
            DOTween.ToAlpha(() => titleText.color, x => titleText.color = x, 1f, 0.5f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(titleDuration);
            DOTween.ToAlpha(() => titleText.color, x => titleText.color = x, 0f, 0.5f).SetUpdate(true);
        }

        // Show story
        if (storyText != null)
        {
            foreach (string line in storyLines)
            {
                storyText.text = line;
                DOTween.ToAlpha(() => storyText.color, x => storyText.color = x, 1f, 0.3f).SetUpdate(true);
                yield return new WaitForSecondsRealtime(storyDuration);
                DOTween.ToAlpha(() => storyText.color, x => storyText.color = x, 0f, 0.3f).SetUpdate(true);
                yield return new WaitForSecondsRealtime(0.3f);
            }
        }

        EndIntro();
    }

    private void SkipIntro()
    {
        StopAllCoroutines();
        EndIntro();
    }

    private void EndIntro()
    {
        isPlaying = false;
        Time.timeScale = 1;

        if (introPanel != null)
            introPanel.gameObject.SetActive(false);

        GameManager.Instance?.StartGame();
        OnIntroComplete?.Invoke();
    }
}
