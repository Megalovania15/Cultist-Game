using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Button selectedButton;

    public string nextLevel;
    public string credits;
    public string mainMenu;
    public string currentLevel;
    public string activeLevel;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentLevel = scene.name;
    }

    public void SelectButton()
    {
        selectedButton.Select();
    }

    public void PlayNextLevel()
    {
        Debug.Log("Go to next level");

        if (nextLevel != null)
            SceneManager.LoadScene(nextLevel);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(credits);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(activeLevel);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void QuitGame()
    {
        print("quit game");
        Application.Quit();
    }
        
    
}
