using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLookAt : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        this.gameObject.transform.LookAt(player);
    }
}
