using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketLauncherAmmo : MonoBehaviour
{
    [Range(1, 3)] [SerializeField] public int rockets;

    public void givePlayerRockets(int rounds)
    {
        gameManager.instance.playerScript.giverockets(rounds);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.giverockets(rockets);
            Destroy(gameObject);
        }
    }
}
