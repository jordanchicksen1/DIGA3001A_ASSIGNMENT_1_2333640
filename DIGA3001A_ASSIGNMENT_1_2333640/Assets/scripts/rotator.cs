using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    public GameObject theRotator;
   
    // Update is called once per frame
    void Update()
    {
        float rotateSpeed = 15f * Time.deltaTime;
        theRotator.transform.Rotate(0f, rotateSpeed, 0f) ;
        Debug.Log("is it working?");
    }
    
}
