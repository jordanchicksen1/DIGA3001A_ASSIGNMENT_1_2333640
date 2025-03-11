 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class flintManager : MonoBehaviour
{
    public int flint;
    public TextMeshProUGUI flintText;
    public GameObject gameFinished;

    [ContextMenu("Increase Flint")]
    public void addFlint()
    {
        flint = flint + 1;
        flintText.text = flint.ToString();
    }

    [ContextMenu("Decrease Flint")]
    public void decreaseFlint()
    {
        flint = flint - 1;
        flintText.text = flint.ToString();
    }

    public void Update()
    {
        if(flint == 4)
        {
            StartCoroutine(EndGame());
            Debug.Log("it should end the game");
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        gameFinished.SetActive(true);
    }
}
