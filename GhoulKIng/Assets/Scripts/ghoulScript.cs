using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ghoulScript : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;

    [Header("-----------------")]


    [Header("Enemy Attributes")]
    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] bool deleteBody;
    [SerializeField] float animBuffer;

    [Header("-----------------")]

    [Header("Weapon Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;



    bool canShoot = true;
    bool playerInRange;

    Vector3 playerDir;
    Vector3 startingPos;
    float StoppingDisOrig;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        StoppingDisOrig = agent.stoppingDistance;
        gameManager.instance.updateEnemyNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));

            playerDir = gameManager.instance.player.transform.position - transform.position;

            agent.SetDestination(gameManager.instance.player.transform.position);
            facePlayer();

            if (playerInRange)
            {
                canSeePlayer();
            }
        }
    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed * 3);
        }
    }

    void canSeePlayer()
    {
        float angle = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            Debug.DrawRay(transform.position, playerDir);

            if (hit.collider.CompareTag("Player") && canShoot && angle <= viewAngle)
                StartCoroutine(shoot());

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            agent.stoppingDistance = StoppingDisOrig;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        anim.SetTrigger("Damage");

        StartCoroutine(flashColor());
        playerInRange = true;


        if (deleteBody != true)
        {
            if (HP <= 0)
            {
                gameManager.instance.checkEnemyKills();
                agent.enabled = false;
                anim.SetBool("Dead", true);
                foreach (Collider col in GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
        }
        else
        {
            gameManager.instance.checkEnemyKills();
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
        if (canShoot)
        {
            canShoot = false;

            anim.SetTrigger("Shoot");

           // yield return new WaitForSeconds(animBuffer);

            Instantiate(bullet, shootPos.transform.position, bullet.transform.rotation);

            yield return new WaitForSeconds(shootRate);

            canShoot = true;
        }
    }
}