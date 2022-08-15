using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] int destoryTime;
    [SerializeField] GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destoryTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            Physics.IgnoreCollision(rb.GetComponent<Collider>(), other);

        }
        else
        {

        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
        }

    }
}
