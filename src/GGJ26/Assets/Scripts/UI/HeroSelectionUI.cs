using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroSelectionUI : MonoBehaviour
{
    [Header("Hero Buttons")]
    [SerializeField] private Button[] heroButtons;
    [SerializeField] private Image[] heroHighlights;

    [Header("Confirm")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private string gameSceneName = "SampleScene";

    private int selectedHeroIndex = -1;

    public static int SelectedHero { get; private set; } = 0;

    private void Start()
    {
        for (int i = 0; i < heroButtons.Length; i++)
        {
            int index = i;
            heroButtons[i].onClick.AddListener(() => SelectHero(index));
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(ConfirmSelection);
            confirmButton.interactable = false;
        }

        UpdateHighlights();
    }

    private void SelectHero(int index)
    {
        selectedHeroIndex = index;
        SelectedHero = index;
        UpdateHighlights();

        if (confirmButton != null)
            confirmButton.interactable = true;
    }

    private void UpdateHighlights()
    {
        for (int i = 0; i < heroHighlights.Length; i++)
        {
            if (heroHighlights[i] != null)
                heroHighlights[i].enabled = (i == selectedHeroIndex);
        }
    }

    private void ConfirmSelection()
    {
        if (selectedHeroIndex >= 0)
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
