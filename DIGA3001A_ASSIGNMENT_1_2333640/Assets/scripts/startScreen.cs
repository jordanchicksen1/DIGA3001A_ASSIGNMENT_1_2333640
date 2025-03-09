using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{
    public GameObject title;
    public GameObject startButton;

    public GameObject playButton;
    public GameObject quitButton;

    public GameObject text1;
    public GameObject next1;
   
    public GameObject text2;
    public GameObject next2;

    public GameObject text3;
    public GameObject next3;

    public GameObject text4;
    public GameObject next4;
    public void StartButton()
    {
        title.SetActive(false);
        startButton.SetActive(false);
        playButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void PlayButton()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        StartCoroutine(PlayButtonPress());
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Next1()
    {
        next1.SetActive(false);
        text1.SetActive(false);
       StartCoroutine(Next2Button());
        text2.SetActive(true);
    }

    public void Next2()
    {
        next2.SetActive(false);
        text2.SetActive(false);
       StartCoroutine(Next3Button());
        text3.SetActive(true);
    }

    public void Next3()
    {
        next3.SetActive(false);
        text3.SetActive(false);
        StartCoroutine(Next4Button());
        text4.SetActive(true);
    }

    public void Next4()
    {
        SceneManager.LoadScene("Game");
    }

    public IEnumerator Next1Button()
    {
        yield return new WaitForSeconds(10f);
        next1.SetActive(true);
    }

    public IEnumerator Next2Button()
    {
        yield return new WaitForSeconds(10f);
        next2.SetActive(true);
    }

    public IEnumerator Next3Button()
    {
        yield return new WaitForSeconds(10f);
        next3.SetActive(true);
    }

    public IEnumerator Next4Button()
    {
        yield return new WaitForSeconds(10f);
        next4.SetActive(true);
    }

    public IEnumerator PlayButtonPress()
    {
        yield return new WaitForSeconds(5f);
        text1.SetActive(true);
        StartCoroutine(Next1Button());
    }
}
