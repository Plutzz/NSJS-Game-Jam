using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script controlls the pause menu

public class PauseMenu : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingMenuUI;

    [Tooltip("Lets other scripts know if the game is paused")]
    private static bool GameIsPaused = false;

    public static PauseMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Lets the player pause and unpause the game using escape
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                CloseMenu();
                Resume();
            }
            else
            {
                OpenMenu();
                Pause();
            }
               
        }
    }

    // Resumes the game
    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Pauses the game
    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Quits to the Main Menu
    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    // Returns true if game is paused and false otherwise
    public static bool getGameIsPaused()
    {
        return GameIsPaused;
    }

    private void OpenMenu()
    {
        pauseMenuUI.SetActive(true);
    }
    private void CloseMenu()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
    }
}