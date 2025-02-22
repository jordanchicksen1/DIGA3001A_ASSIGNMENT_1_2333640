using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class campfireManager : MonoBehaviour
{
    public int campfire;
    public TextMeshProUGUI campfireText;

    
    public void addCampfire()
    {
        campfire = campfire + 1;
        campfireText.text = campfire.ToString();
    }

}
