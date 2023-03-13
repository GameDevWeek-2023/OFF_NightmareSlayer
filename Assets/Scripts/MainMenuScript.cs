using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject creditspanel;
    public GameObject settingsPanel;

    public void play()
    {
        //SceneManager.LoadScene("Game");
    }

    public void settings()
    {
        settingsPanel.SetActive(true);
    }

    public void credits()
    {
        creditspanel.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
}
