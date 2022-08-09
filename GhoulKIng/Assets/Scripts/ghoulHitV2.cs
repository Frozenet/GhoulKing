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

    [Header("----------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float shootRate = 2f;
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
    [SerializeField] bool playerInRange;
    Vector3 playerDir;
    Vector3 startingPos;
    float StoppingDistOrig;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        StoppingDistOrig = agent.stoppingDistance;
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
            /* else if (agent.remainingDistance < 0.1f)
                 StartCoroutine(roam());*/
        }
    }
    /* IEnumerator roam()
     {
         agent.stoppingDistance = 0;
         Vector3 randomDir = Random.insideUnitSphere * roamRadius;
         randomDir += startingPos;

         NavMeshHit hit;
         NavMesh.SamplePosition(randomDir, out hit, roamRadius, 1);
         NavMeshPath path = new NavMeshPath();

         agent.CalculatePath(hit.position, path);
         agent.SetPath(path);
         yield return new WaitForSeconds(3);
     }*/
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
        canShoot = true;
        float angle = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            Debug.DrawRay(transform.position, playerDir);
            if (hit.collider.CompareTag("Player") && canShoot && angle <= viewAngle)
            {
                StartCoroutine(shoot());
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //atack
            StartCoroutine(shoot());
            playerInRange = true;
            // canShoot = true;
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
        HP -= dmg;
        playerInRange = true;
        //aud.PlayOneShot(playerHurt[Random.Range(0, playerHurt.Length)], playerHurtVol);\
        aud.PlayOneShot(enemyTakeDamage[0], damageAudVol);
        StartCoroutine(flashColor());
        if (HP <= 0)
        {
            gameManager.instance.checkEnemyKills();
            agent.enabled = false;
            anim.SetBool("Dead", true);

            foreach (Collider col in GetComponents<Collider>())
                col.enabled = false;

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
        if (canShoot == true)
        {
            canShoot = false;

            anim.SetTrigger("Shoot");// Lets us use shoot animation
           yield return new WaitForSeconds(AttackAnimBuffer);
           // //-------------

            Instantiate(bullet, shootPos.transform.position, Quaternion.identity);

            // 

            yield return new WaitForSeconds(shootRate);

            canShoot = true;
        }
    }



}