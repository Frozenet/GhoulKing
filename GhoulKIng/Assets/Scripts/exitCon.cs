using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitCon : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.checkKeys();
        }
    }
}
