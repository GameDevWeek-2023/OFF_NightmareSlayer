using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    public GameObject creditspanel, settingsPanel;

    public GameObject settingsButton, creditsButton, afterSettingsButton, afterCreditsButton;
    
    

    public void play()
    {
        Debug.Log("play");
        SceneManager.LoadScene("Realm");
    }
    
    public void exit()
    {
        Application.Quit();
    }

    public void creditsOpen()
    {
        creditspanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }

    public void creditsClose()
    {
        creditspanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(afterCreditsButton);
    }

    public void settingsOpen()
    {
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsButton);
    }

    public void settingClose()
    {
        settingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(afterSettingsButton);
    }
}
