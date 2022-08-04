using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int pushBackAmount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if (!other.isTrigger)
        {
            if (other.gameObject.GetComponent<IDamageable>() != null)
            {
                if (other.CompareTag("Player"))
                {
                    gameManager.instance.playerScript.pushback = (gameManager.instance.player.transform.position - transform.position) * pushBackAmount;
                }
                IDamageable isDamageable = other.GetComponent<IDamageable>();
                isDamageable.takeDamage(damage);
            }

        }

=======
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pushback = (gameManager.instance.player.transform.position - transform.position) * pushBackAmount;
            if (other.GetComponent<IDamageable>() != null)
            {
                IDamageable isDamageable = other.GetComponent<IDamageable>();
                isDamageable.takeDamage(damage);
            }
        }
>>>>>>> e9a3e928c893fe58e21a8d13b2cbc95d06350a37
    }
}
