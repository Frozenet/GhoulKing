using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotGunAmmo : MonoBehaviour
{
    [Range(1, 10)] [SerializeField] public int shells;

    public void givePlayerShells(int shells)
    {
        gameManager.instance.playerScript.giveShots(shells);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.giveShots(shells);
        }
        for (int i = 0; i < shells; i++)
        {
           gameManager.instance.playerScript.respawn();
           gameManager.instance.restart();
        }
    }
}
