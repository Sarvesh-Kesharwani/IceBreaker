using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int CurrentLevel;
    public int LoadLevel;

    public GameObject Player;

    public List<GameObject> gameobjectToHide;
    public List<GameObject> gameobjectToShow;

    public GameObject Console;
    public GameObject consoleOnButton;
    public GameObject consoleOffButton;


    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadCurrentLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadLevelN()
    {
        SceneManager.LoadScene(LoadLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowPanels()
    {
        for (int i = 0; i < gameobjectToShow.Count; i++)
        {
            gameobjectToShow[i].SetActive(true);
        }
    }

    public void HidePanels()
    {
        for (int i = 0; i < gameobjectToHide.Count; i++)
        {
            gameobjectToHide[i].SetActive(false);
        }
    }

    public void PlayerActive(bool state)
    {
        Player.SetActive(state);
    }

    public void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OnConsoleOffClicked()
    {
        Console.gameObject.SetActive(true);
        consoleOnButton.SetActive(true);
        consoleOffButton.SetActive(false);

    }
    public void OnConsoleOnClicked()
    {
        Console.gameObject.SetActive(false);
        consoleOnButton.SetActive(false);
        consoleOffButton.SetActive(true);
    }
}