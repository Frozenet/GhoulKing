using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBall : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] int destoryTime;

    void Start()
    {
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destoryTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pushback = (gameManager.instance.player.transform.position - transform.position) * damage;

            if(other.GetComponent<IDamageable>() != null)
            {
                IDamageable isdamageable = other.GetComponent<IDamageable>();
                isdamageable.takeDamage(damage);
            }
        }
    }
}
