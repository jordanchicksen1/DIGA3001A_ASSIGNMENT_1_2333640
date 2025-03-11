using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class campfireManager : MonoBehaviour
{
    public int campfire;
    public TextMeshProUGUI campfireText;
    public GameObject gameFinished;
    
    public void addCampfire()
    {
        campfire = campfire + 1;
        campfireText.text = campfire.ToString();
    }

    public void Update()
    {
        if (campfire > 3.99)
        {
            StartCoroutine(EndGame());
            Debug.Log("it should end the game");
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
       
        gameFinished.SetActive(true);
    }
}
