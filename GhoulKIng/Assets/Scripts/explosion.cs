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
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.playerScript.pushback = (gamemanager.instance.player.transform.position - transform.position) * pushBackAmount;
            if (other.GetComponent<IDamageable>() != null)
            {
                IDamageable isDamageable = other.GetComponent<IDamageable>();
                isDamageable.takeDamage(damage);
            }
        }
    }
}
