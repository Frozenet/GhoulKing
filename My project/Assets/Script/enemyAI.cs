using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamagable
{
    [Header("Components")]
    [SerializeField]NavMeshAgent agent;
    [SerializeField] Renderer rend;

    [Header("------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int HP;

    [Header("------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    bool canShoot = true;
    bool playerRange;

    // Start is called before the first frame update
    void Start()
    {
       
        gamemanager.instance.playerScript.takeDamage(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gamemanager.instance.player.transform.position);

        if (agent.remainingDistance<= agent.stoppingDistance && canShoot)
        {
            StartCoroutine(shoot());
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        StartCoroutine(flashColor());

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
    }
    IEnumerator shoot()
    {
        canShoot = false;
        Instantiate(bullet, transform.position, bullet.transform.rotation);
        if (agent.remainingDistance <= agent.stoppingDistance && canShoot)
        {
            StartCoroutine(shoot());
        }
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }
}
