using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void start()
    {
        SceneManager.LoadScene("Game");
    }
    public void quit()
    {
        Application.Quit();
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void levelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }
}
