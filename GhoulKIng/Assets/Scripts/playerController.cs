using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;
    [Header("-----------------")]

    [Header("Player Attributes")]
    [Range(5, 20)][SerializeField] int HP;
    [Range(1, 15)][SerializeField] float playerSpeed;
    [Range(0, 4f)][SerializeField] float sprintMult;
    [Range(1, 5)][SerializeField] int jumps;
    [Range(1, 10)][SerializeField] float jumpHeight;
    [Range(15, 30)][SerializeField] float gravityValue;
    [Header("-----------------")]

    [Header("Player Weapon Stats")]
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int weaponDamage;

    [Header("-----------------")]
    [Header("Effects")]
    [SerializeField] GameObject hitEffectSpark;
    [SerializeField] GameObject muzzleFlash;

    [Header("-----------------")]
    [Header("Physics")]
    public Vector3 pushback = Vector3.zero;
    [SerializeField] int pushResolve;

    [Header("-----------------")]//new
    [Header("Audio")]//new
    public AudioSource aud;//new
    [SerializeField] AudioClip[] gunshot;//new
    [Range(0, 1)][SerializeField] float gunshotVol;//new
    [SerializeField] AudioClip[] playerHurt;//new
    [Range(0, 1)][SerializeField] float playerHurtVol;//new
    [SerializeField] AudioClip[] playerFootsteps;//new
    [Range(0, 1)][SerializeField] float playerFootstepsVol;//new

    bool isSprinting = false;
    float playerSpeedOrig;
    int timesjumped;
    Vector3 playerVelocity;
    Vector3 move;

    bool canShoot = true;
    int HPOrig;
    Vector3 playerSpawnPos;
    bool footsetpPlaying;//new

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        playerSpawnPos = transform.position;
    }

    void Update()
    {
        if (!gamemanager.instance.paused)
        {
            pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushResolve);
            movePlayer();
            sprint();
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

    IEnumerator shoot()
    {
        RaycastHit hit;

        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);

        if (Input.GetButton("Shoot") && canShoot)
        {

            canShoot = false;

            aud.PlayOneShot(gunshot[Random.Range(0, gunshot.Length)], gunshotVol);//new
<<<<<<< HEAD

            //if (shotgun)
            //{
            //
            //}
            //else
            //{
            //
            //}
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
=======
            for (int i = 0; i < 12; i++)
>>>>>>> cae57f2a09125aeeffdcebb8df5a5554140ed6aa
            {

                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), out hit))
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
<<<<<<< HEAD
            }

            muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            muzzleFlash.SetActive(false);


            yield return new WaitForSeconds(shootRate);
            canShoot = true;

=======
            }
                muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                muzzleFlash.SetActive(true);
                yield return new WaitForSeconds(0.05f);
                muzzleFlash.SetActive(false);


                yield return new WaitForSeconds(shootRate);
                canShoot = true;

            }
>>>>>>> cae57f2a09125aeeffdcebb8df5a5554140ed6aa
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
                gamemanager.instance.playerDead();
            }
        }
        IEnumerator damageFlash()
        {
            gamemanager.instance.playerDamageFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            gamemanager.instance.playerDamageFlash.SetActive(false);

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
            gamemanager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;

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
