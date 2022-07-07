using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] int destoryTime;
    [SerializeField] GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (gamemanager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destoryTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            IDamageable isDamageable = other.GetComponent<IDamageable>();
            isDamageable.takeDamage(damage);
        }
        Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
        Destroy(gameObject);
    }

}
