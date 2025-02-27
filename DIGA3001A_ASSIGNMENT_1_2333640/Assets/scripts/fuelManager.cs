using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class fuelManager : MonoBehaviour
{
    public int fuel;
    public TextMeshProUGUI fuelText;


    public void addFuel()
    {
        fuel = fuel + 1;
        fuelText.text = fuel.ToString();
    }

    public void subtractFuel()
    {
        fuel = fuel - 1;
        fuelText.text = fuel.ToString();
    }
}
