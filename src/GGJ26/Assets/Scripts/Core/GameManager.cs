using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Playing, Paused, Won, Lost }

    public GameState CurrentState { get; private set; } = GameState.Menu;

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void PlayerWon()
    {
        SetState(GameState.Won);
    }

    public void PlayerLost()
    {
        SetState(GameState.Lost);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
