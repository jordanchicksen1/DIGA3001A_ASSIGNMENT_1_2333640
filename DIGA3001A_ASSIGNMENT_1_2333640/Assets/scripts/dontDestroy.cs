using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
