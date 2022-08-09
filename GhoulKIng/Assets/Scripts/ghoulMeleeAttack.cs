using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghoulMeleeAttack : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [Range (0.01f, 10)][SerializeField] float destoryTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destoryTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            IDamageable isDamageable = other.GetComponent<IDamageable>();
            isDamageable.takeDamage(damage);
        }
        Destroy(gameObject);
    }

}
