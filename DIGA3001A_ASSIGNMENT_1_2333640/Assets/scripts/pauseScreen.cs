using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseScreen : MonoBehaviour
{
    public GameObject pausePage;
    public GameObject controlsPage;
    public GameObject systemPage;
    public GameObject controlsButton;
    public GameObject systemsButton;
    public GameObject quitButton;

    public playerControls playerControls;

    public void Quit()
    {
        Application.Quit();
    }

    public void Controls()
    {
        controlsPage.SetActive(true);
    }

    public void Systems()
    {
        systemPage.SetActive(true);
    }
  
    public void PauseOff()
    {
        pausePage.SetActive(false);
        playerControls.isPaused = false;
        pausePage.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("should unpaused");
        controlsPage.SetActive(false);
        systemPage.SetActive(false);
    }

    public void Back()
    {
       systemPage.SetActive(false);
       controlsPage.SetActive(false);
    }


}
