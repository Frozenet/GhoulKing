using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ghoulHitV2 : MonoBehaviour, IDamageable
{
    [Header("Components")]

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;

    [Header("----------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamRadius;
    [SerializeField] float AttackAnimBuffer;
    public float attackTimer;
    public float attackTime;

    [Header("----------------------------------")]
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
    [SerializeField] bool playerInRange = false;
    Vector3 playerDir;
    Vector3 startingPos;
    float StoppingDistOrig;
    float speedOrig;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        StoppingDistOrig = agent.stoppingDistance;
        //gameManager.instance.updateEnemyNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {

            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));

            playerDir = gameManager.instance.player.transform.position - transform.position;
            playerDir.Normalize();
            agent.SetDestination(gameManager.instance.player.transform.position);
            facePlayer();

        }
        if (playerInRange)
        {
            canSeePlayer();
        }
    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            var rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }
    
    void canSeePlayer()
    {
        float angle = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;

        if(HP != 0)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), playerDir, out hit))
            {
                Debug.DrawRay(transform.position+new Vector3(0,1,0), playerDir,Color.magenta);
                if (hit.collider.CompareTag("Player") && canShoot && angle <= viewAngle)
                {
                    shoot();
                }
            }
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            agent.stoppingDistance = StoppingDistOrig;

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
        speedOrig = agent.speed;
        HP -= dmg;
        aud.PlayOneShot(enemyTakeDamage[0], damageAudVol);

        agent.speed = 0;
        anim.SetTrigger("Damage");

        if (HP <= 0)
        {
            gameManager.instance.checkEnemyKills();
            agent.enabled = false;
            anim.SetBool("Dead", true);

            foreach (Collider col in GetComponents<Collider>())
                col.enabled = false;

            canShoot = false;
        }
    }

    void shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            anim.SetTrigger("Shoot");

            StartCoroutine(attackTimerDelay());
        }
    }

    void fireProjectile()
    {
        Instantiate(bullet, shootPos.transform.position, bullet.transform.rotation);
    }

    IEnumerator attackTimerDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(attackTime);
        canShoot = true;
    }

    void noSpeed()
    {
        agent.speed = 0;
    }

    void haveSpeed()
    {
        agent.speed = speedOrig;
    }
}