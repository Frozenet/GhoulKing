using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skeletonScript : MonoBehaviour, IDamageable
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
    [SerializeField] int roamRadius;
    [SerializeField] bool deleteBody;
    [SerializeField] float AttackAnimBuffer;
    public float attackTimer;
    public float attackTime;

    [Header("-----------------")]

    [Header("Weapon Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;

    [Header("-----------------")]
    [Header("Audio")]
    public AudioSource aud;
    [SerializeField] AudioClip[] enemyidle;
    [Range(0, 1)][SerializeField] float idleAudVol;
    [SerializeField] AudioClip[] enemyTakeDamage;
    [Range(0, 1)][SerializeField] float damageAudVol;
    [SerializeField] AudioClip[] deadAud;
    [Range(0, 1)][SerializeField] float deadAudVol;



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
        //gameManager.instance.updateEnemyNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));

            playerDir = gameManager.instance.player.transform.position - transform.position;

            if (playerInRange)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                canSeePlayer();
                facePlayer();
            }
            else if (agent.remainingDistance < 0.1f)
                roam();
        }
    }

    void roam()
    {
        agent.stoppingDistance = 0;

        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, roamRadius, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            var rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed * 3);
        }
    }

    void canSeePlayer()
    {
        float angle = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0,0.5f, 0), playerDir, out hit))
        {
            Debug.DrawRay(transform.position, playerDir);

            if (hit.collider.CompareTag("Player") && canShoot && angle <= viewAngle)
                shoot();

        }
    }

    public void OnTriggerStay(Collider other)
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

        aud.PlayOneShot(enemyTakeDamage[0], damageAudVol);

        anim.SetTrigger("Damage");

        if (HP <= 0)
        {
            if (deleteBody != true)
            {
                gameManager.instance.checkEnemyKills();
                agent.enabled = false;
                anim.SetBool("Dead", true);
                foreach (Collider col in GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
            else
            {
                gameManager.instance.checkEnemyKills();
                Destroy(gameObject);
            }
        }
    }
    void shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            anim.SetTrigger("Shoot");

            Instantiate(bullet, shootPos.transform.position, bullet.transform.rotation);
            StartCoroutine(attackTimerDelay());
        }
    }

    IEnumerator attackTimerDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(attackTime);
        canShoot = true;
    }
}
