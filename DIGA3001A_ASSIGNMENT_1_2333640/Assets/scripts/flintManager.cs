 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class flintManager : MonoBehaviour
{
    public int flint;
    public TextMeshProUGUI flintText;

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
}
