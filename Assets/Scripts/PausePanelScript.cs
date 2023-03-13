using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelScript : MonoBehaviour
{
    public GameObject settingspanel;

    public void resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void settings()
    {
        settingspanel.SetActive(true);
    }

    public void exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
