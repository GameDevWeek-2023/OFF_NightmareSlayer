using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PausePanelScript : MonoBehaviour
{
    public GameObject settingspanel;
    public GameObject settingsButton, afterSettingsButton, firstChosenButton;


    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstChosenButton);
    }

    public void resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        PlayerScript.instance.canMove = true;
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void openSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsButton);
        settingspanel.SetActive(true);
    }

    public void closeSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(afterSettingsButton);
        settingspanel.SetActive(false);
    }

    public void exit()
    {
        SwitchableObject.RemoveAll();
        Fruit.RemoveAll();
        ReplaceEnemy.RemoveAll();
        
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        settingspanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstChosenButton);
    }
}
