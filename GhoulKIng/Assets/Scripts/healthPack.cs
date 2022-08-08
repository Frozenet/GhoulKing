using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPack : MonoBehaviour
{
    [Range(1, 10)] [SerializeField] int amount;

    public void givePlayerHP(int amount)
    {
        gameManager.instance.playerScript.giveHP(amount);
    }
    public void OnTriggerEnter(Collider other)
    {
       

            if (other.CompareTag("Player"))
            {
                gameManager.instance.playerScript.giveHP(amount);
                Destroy(gameObject);
            }
        
    }
}
