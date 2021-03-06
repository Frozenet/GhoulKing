using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;
    [Header("-----------------")]

    [Header("Player Attributes")]
    [Range(5, 20)] [SerializeField] int HP;
    [Range(1, 15)] [SerializeField] float playerSpeed;
    [Range(0, 4f)] [SerializeField] float sprintMult;
    [Range(1, 5)] [SerializeField] int jumps;
    [Range(1, 10)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
    [Header("-----------------")]

    [Header("Player Weapon Stats")]
    [Range(0.1f, 3)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int weaponDamage;
    //public GameObject pistol;//old
    //public GameObject shotgun;//old
    //public GameObject currentWeapon;//old
    [SerializeField] GameObject gunModel;
    public List<gunStats> gunstat = new List<gunStats>();

    [Header("-----------------")]
    [Header("Effects")]
    [SerializeField] GameObject hitEffectSpark;
    [SerializeField] GameObject muzzleFlash;

    [Header("-----------------")]
    [Header("Physics")]
    public Vector3 pushback = Vector3.zero;
    [SerializeField] int pushResolve;

    [Header("-----------------")]
    [Header("Audio")]
    public AudioSource aud;
    [SerializeField] AudioClip[] gunshot;
    [Range(0, 1)] [SerializeField] float gunshotVol;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)] [SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerFootsteps;
    [Range(0, 1)] [SerializeField] float playerFootstepsVol;

    bool isSprinting = false;
    float playerSpeedOrig;
    int timesjumped;
    Vector3 playerVelocity;
    Vector3 move;

    bool canShoot = true;
    int HPOrig;
    Vector3 playerSpawnPos;
    bool footsetpPlaying;
    public int weaponType = 0;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        playerSpawnPos = transform.position;
        //currentWeapon = shotgun;
    }

    void Update()
    {
        if (!gameManager.instance.paused)
        {
            pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushResolve);
            movePlayer();
            sprint();
            //weaopnChoice();
            StartCoroutine(shoot());
            StartCoroutine(playFootsteps());//new
        }
    }

    private void movePlayer()
    {
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            playerVelocity.y -= 3;
        }

        //if we land reset player Y velocity and the jump counter
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            timesjumped = 0;
            playerVelocity.y = 0f;
        }
        //get input from unity input system
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        // add our move vector  into charector controller move
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesjumped < jumps)
        {
            timesjumped++;
            playerVelocity.y = jumpHeight;
        }

        //add gravity 
        playerVelocity.y -= gravityValue * Time.deltaTime;

        //add gravity back into the charector controller move
        controller.Move((playerVelocity + pushback) * Time.deltaTime);
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMult;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }
    IEnumerator playFootsteps()//new function
    {
        if (controller.isGrounded && move.normalized.magnitude > .4f && !footsetpPlaying)
        {
            footsetpPlaying = true;
            aud.PlayOneShot(playerFootsteps[Random.Range(0, playerFootsteps.Length)], playerFootstepsVol);
            if (isSprinting)
                yield return new WaitForSeconds(.4f);
            else
                yield return new WaitForSeconds(.2f);

            footsetpPlaying = false;
        }
    }

    /*void weaopnChoice()
    {
        if (Input.GetKeyDown("1"))
        {
            weaponType = 0;
            currentWeapon.SetActive(false);
            currentWeapon = shotgun;
            currentWeapon.SetActive(true);
        }
        if (Input.GetKeyDown("2"))
        {
            weaponType = 1;
            currentWeapon.SetActive(false);
            currentWeapon = pistol;
            currentWeapon.SetActive(true);
        }
    }*/

    IEnumerator shoot()
    {
        RaycastHit hit;

        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);

        if (Input.GetButton("Shoot") && canShoot)
        {
            canShoot = false;

            aud.PlayOneShot(gunshot[Random.Range(0, gunshot.Length)], gunshotVol);//new

            //if (weaponType == 0) // 1 is shotgun 
            //{
            //    for (int i = 0; i < 12; i++)
            //    {
            //        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), out hit))
            //        {
            //            Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
            //            if (hit.collider.GetComponent<IDamageable>() != null)
            //            {
            //                IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
            //                if (hit.collider is SphereCollider)
            //                {
            //                    isDamageable.takeDamage(weaponDamage * 5);
            //                }
            //                else
            //                {
            //                    isDamageable.takeDamage(weaponDamage);
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (weaponType == 1)// 2 is pistol/rifle
            //{
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
                {
                    Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
                    if (hit.collider.GetComponent<IDamageable>() != null)
                    {
                        IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
                        if (hit.collider is SphereCollider)
                        {
                            isDamageable.takeDamage(weaponDamage * 5);
                        }
                        else
                        {
                            isDamageable.takeDamage(weaponDamage);
                        }
                    }
                }
            //}

            muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            muzzleFlash.SetActive(false);
            yield return new WaitForSeconds(shootRate);
            canShoot = true;
        }
    }

    public void gunPickup(float fireRate, int damage, GameObject model, gunStats stats)
    {
        shootRate = fireRate;
        weaponDamage = damage;
        gunModel.GetComponent<MeshFilter>().sharedMesh = model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = model.GetComponent<MeshRenderer>().sharedMaterial;
        gunstat.Add(stats);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        aud.PlayOneShot(playerHurt[Random.Range(0, gunshot.Length)], playerHurtVol);//new

        updatePlayerHP();
        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            //kill player
            gameManager.instance.playerDead();
        }
    }
    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);

    }
    public void giveHP(int amount)
    {
        if (HP < HPOrig)
        {
            HP += amount;
        }
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerHP();

    }
    public void updatePlayerHP()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;

    }
    public void respawn()
    {
        HP = HPOrig;
        controller.enabled = false;
        transform.position = playerSpawnPos;
        controller.enabled = true;
        pushback = Vector3.zero;
        updatePlayerHP();

    }

}
