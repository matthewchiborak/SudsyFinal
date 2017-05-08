using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public Image loadingScreen;
    public Image controlScreen;
    public GameObject backButton;
    public Image storyScreen;

    public void startGame()
    {
        //Load the gameplay scene
        loadingScreen.enabled = true;
        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }

    public void newGame()
    {
        //Load the gameplay scene
        loadingScreen.enabled = true;

        //Reset the savefile
        System.IO.File.WriteAllText("Assets/Last_level.txt", "1");

        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }

    public void quitGame()
    {
        //Closes the application
        Application.Quit();
    }

    public void howToPlayClicked()
    {
        controlScreen.enabled = true;
        backButton.SetActive(true);
    }

    public void storyClicked()
    {
        storyScreen.enabled = true;
        backButton.SetActive(true);
    }

    public void backFromControls()
    {
        controlScreen.enabled = false;
        storyScreen.enabled = false;
        backButton.SetActive(false);
    }

}
