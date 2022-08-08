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
        if (gameManager.instance.playerScript.shotgunAmmo < gameManager.instance.playerScript.shotgunAmmoMax)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.instance.playerScript.giveShots(shells);
                Destroy(gameObject);

            }
        }
    }
}
