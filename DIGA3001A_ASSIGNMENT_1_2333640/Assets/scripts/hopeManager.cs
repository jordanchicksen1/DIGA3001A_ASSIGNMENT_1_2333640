using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class hopeManager : MonoBehaviour
{
    public float maxHope = 100f;
    public float currentHope;
    public Image hopeBar;
    public playerControls playerControls;
    public Light Light;
    public float maxFlame = 100f;
    public float minFlame = 0f;

    public void Start()
    {
        currentHope = maxHope;
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
            }
        }

        if(playerControls.increaseHope == true) 
        { 
            currentHope += Time.deltaTime;
            updateHopeBar();
            if( Light.intensity <= maxFlame)
            {
                Light.intensity += Time.deltaTime;
            }
        }
        
    }
}
