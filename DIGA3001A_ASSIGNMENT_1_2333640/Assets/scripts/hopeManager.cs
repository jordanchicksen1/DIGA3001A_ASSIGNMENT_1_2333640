using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class hopeManager : MonoBehaviour
{
    public float maxHope = 100f;
    public float currentHope;
    public Image hopeBar;
    public playerControls playerControls;
    public Light Light;
    public float maxFlame = 100f;
    public float minFlame = 0f;

    //death screen
    public GameObject endScreen;

    public void Start()
    {
        currentHope = maxHope;
    }

    public void UseFuel()
    {
        currentHope = currentHope + 20f;
        updateHopeBar();
    }
    
    public void EnemyAttack()
    {

    }
    public void updateHope(float amount)
    {
        currentHope += amount;
        updateHopeBar();
    }

    public void updateHopeBar()
    {
        float targetFillAmount = currentHope / maxHope;
        hopeBar.fillAmount = targetFillAmount;
    }

    public void Update()
    {
        if(playerControls.decreaseHope == true)
        {
            currentHope -= Time.deltaTime;
            updateHopeBar();
            if( Light.intensity >= minFlame)
            {
                Light.intensity -= Time.deltaTime;
               // Light.range -= Time.deltaTime * 0.02f;
            }
        }

        if(playerControls.increaseHope == true) 
        { 
            currentHope += Time.deltaTime;
            updateHopeBar();
            if( Light.intensity <= maxFlame)
            {
                Light.intensity += Time.deltaTime;
               // Light.range += Time.deltaTime * 0.02f;
            }
        }

       CheckHope();
        
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CheckHope()
    {
        if (currentHope <= 0f)
        {
            endScreen.SetActive(true);
        }
    }
}
