using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int pushBackAmount;
    [SerializeField] float Damageframe;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Nocollider());
    }
    IEnumerator Nocollider()
    {
        yield return new WaitForSeconds(Damageframe);
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
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

    }
}
