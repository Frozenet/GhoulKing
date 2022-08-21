using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;
    [Header("-----------------")]

    [Header("Player Attributes")]
    [Range(5, 20)][SerializeField] public int HP;
    [Range(1, 15)][SerializeField] float playerSpeed;
    [Range(0, 4f)][SerializeField] float sprintMult;
    [Range(1, 5)][SerializeField] int jumps;
    [Range(1, 10)][SerializeField] float jumpHeight;
    [Range(15, 30)][SerializeField] float gravityValue;
    [Header("-----------------")]

    [Header("Player Weapon Stats")]
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int weaponDamage;
    [Range(5, 100)][SerializeField] float range;

    [Range(1, 90)][SerializeField] public int shotgunAmmoMax;
    [Range(1, 90)][SerializeField] public int rocketAmmoMax;

    public int shotgunAmmo;
    public int rocketAmmo;

    public GameObject pistol;
    public GameObject shotgun;
    public GameObject RocketLancher;
    public GameObject shootpos;

    [SerializeField] GameObject rocket;
    public GameObject currentWeapon;
    [SerializeField] GameObject gunModel;
    public List<gunStats> gunstat = new List<gunStats>();

    [Header("-----------------")]
    [Header("weapon lerps")]

    float current = 0, target = 0;

    [SerializeField] GameObject pistolOrig;
    [SerializeField] GameObject pistolRecoil;
    [SerializeField] GameObject shotgunOrig;
    [SerializeField] GameObject shotgunRecoil;
    [SerializeField] GameObject rocketLauncherOrig;
    [SerializeField] GameObject rocketLauncheRecoil;
    [SerializeField] GameObject RPGhead;



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
    [SerializeField] AudioClip[] PistolAud;
    [Range(0, 1)][SerializeField] float PistolAudVol;
    [SerializeField] AudioClip[] ShotgunAud;
    [Range(0, 1)][SerializeField] float ShotgunAudVol;
    [SerializeField] AudioClip[] RocketLancherAud;
    [Range(0, 1)][SerializeField] float RocketLancherAudVol;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)][SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerFootsteps;
    [Range(0, 1)][SerializeField] float playerFootstepsVol;

    bool isSprinting = false;
    float playerSpeedOrig;
    int timesjumped;
    Vector3 playerVelocity;
    Vector3 move;
    public int playerDeathCount = 0;

    bool canShoot = true;
    bool triggerPull = false;
    bool isshooting = false;
    public int HPOrig;
    Vector3 playerSpawnPos;
    bool footsetpPlaying;
    public int weaponType;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        playerSpawnPos = transform.position;
        shotgunAmmo = 0;
        rocketAmmo = 0;
        currentWeapon = pistol;
        weaponType = 0;
        shootRate = 0.5f;
        weaponDamage = 1;
        range = 50;
    }

    void Update()
    {
        if (!gameManager.instance.paused)
        {
            pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushResolve);
            movePlayer();
            sprint();
            if (weaponType != gameManager.instance.playerWeaponSwap.selectedweapon)
            {
                weaopnChoice();
            }
            gameManager.instance.updateAmmo();
            
            StartCoroutine(shoot());

            if (isshooting || triggerPull)
            {
                if (triggerPull)
                {
                    current = 1;
                    triggerPull = false;
                }
                if (currentWeapon == pistol)
                {
                    StartCoroutine(pistolLerp());
                }
                if (currentWeapon == shotgun)
                {
                    StartCoroutine(shotgunLerp());
                }
                if (currentWeapon == RocketLancher)
                {
                    StartCoroutine(rocketlauncherLerp());
                }

            }
            StartCoroutine(playFootsteps());//new
            gameManager.instance.updateAmmo();
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

    void weaopnChoice()
    {
        weaponType = gameManager.instance.playerWeaponSwap.selectedweapon;
        if (weaponType == 0)
        {
            currentWeapon.SetActive(false);
            shootRate = 0.5f;
            weaponDamage = 1;
            range = 50;
            currentWeapon = pistol;
            currentWeapon.SetActive(true);
        }
        if (weaponType == 1)
        {
            currentWeapon.SetActive(false);
            shootRate = 1f;
            weaponDamage = 1;
            range = 20;
            currentWeapon = shotgun;
            currentWeapon.SetActive(true);

        }
        if (weaponType == 2)
        {
            currentWeapon.SetActive(false);
            shootRate = 1f;
            weaponDamage = 10;
            currentWeapon = RocketLancher;
            currentWeapon.SetActive(true);
        }
    }

    public void objLerp(GameObject obj, GameObject objOrigPos, GameObject objRecoilPos, float curPos)
    {
        obj.transform.position = Vector3.Lerp(objOrigPos.transform.position, objRecoilPos.transform.position, curPos);
        obj.transform.rotation = Quaternion.Lerp(objOrigPos.transform.rotation, objRecoilPos.transform.rotation, curPos);
    }

    IEnumerator pistolLerp()
    {
        isshooting = true;

        if (current == 0 && isshooting)
        {
            isshooting = false; 
        }
        float recoilTime = shootRate;
        objLerp(currentWeapon, pistolOrig, pistolRecoil, current);
        current = Mathf.MoveTowards(current, target, (recoilTime / (recoilTime * recoilTime)) * Time.deltaTime);

        yield return null;
    }

    IEnumerator shotgunLerp()
    {
        isshooting = true;

        if (current == 0 && isshooting)
        {
            isshooting = false;
        }
        float recoilTime = shootRate;
        objLerp(currentWeapon, shotgunOrig, shotgunRecoil, current);
        current = Mathf.MoveTowards(current, target, (recoilTime / (recoilTime * recoilTime)) * Time.deltaTime);

        yield return null;
    }

    IEnumerator rocketlauncherLerp()
    {
        isshooting = true;

        if (current == 0 && isshooting)
        {
            isshooting = false;
        }
        float recoilTime = shootRate;
        objLerp(currentWeapon, rocketLauncherOrig, rocketLauncheRecoil, current);
        current = Mathf.MoveTowards(current, target, (recoilTime / (recoilTime * recoilTime)) * Time.deltaTime);

        yield return null;
    }

    IEnumerator shoot()
    {

        RaycastHit hit;

        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);

        if (Input.GetButton("Shoot") && canShoot)
        {
            


            if (weaponType == 0)// 0 is pistol
            {
                canShoot = false;
                triggerPull = true;
                aud.PlayOneShot(PistolAud[Random.Range(0, PistolAud.Length)], PistolAudVol);//new

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, range))
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
            }
            else if (weaponType == 1) // 1 is shotgun 
            {
                if (shotgunAmmo != 0)
                {
                    shotgunAmmo--;
                    canShoot = false;
                    triggerPull = true;
                    aud.PlayOneShot(ShotgunAud[0], ShotgunAudVol);
                    for (int i = 0; i < 12; i++)
                    {
                        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), out hit, range))
                        {
                            Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
                            if (hit.collider.GetComponent<IDamageable>() != null)
                            {
                                IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
                                if (hit.collider is SphereCollider)
                                {
                                    isDamageable.takeDamage(weaponDamage);
                                }
                                else
                                {
                                    isDamageable.takeDamage(weaponDamage);
                                }
                            }
                        }
                    }
                }
                else
                {
                    canShoot = false;
                    aud.PlayOneShot(ShotgunAud[1], ShotgunAudVol);
                }
            }
            else if (weaponType == 2)// 2 is rockectlancher
            {
                if (rocketAmmo != 0)
                {
                    rocketAmmo--;
                    canShoot = false;
                    triggerPull = true;
                    aud.PlayOneShot(RocketLancherAud[0], RocketLancherAudVol);//new
                    Instantiate(rocket, shootpos.transform.position, shootpos.transform.rotation);

                    RPGhead.SetActive(false);
                    yield return new WaitForSeconds(shootRate);
                    RPGhead.SetActive(true);
                    
                }
                else
                {
                    canShoot = false;
                    aud.PlayOneShot(RocketLancherAud[1], RocketLancherAudVol);
                }

            }
            gameManager.instance.updateAmmo();
            //muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            //muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            //muzzleFlash.SetActive(false);
            yield return new WaitForSeconds(shootRate);
            canShoot = true;

        }
    }
    public void giveShots(int shells)
    {
        if (shotgunAmmo < shotgunAmmoMax)
        {
            shotgunAmmo += shells;
        }
        if (shotgunAmmo > shotgunAmmoMax)
        {
            shotgunAmmo = shotgunAmmoMax;
        }
        updatePlayerShells();

    }
    public void updatePlayerShells()
    {
        //shotgunAmmo = shotgunAmmoMax;
        float shellPercent = ((float)shotgunAmmo / (float)shotgunAmmoMax);
        gameManager.instance.shotgunAmmo.text = shellPercent.ToString("F0");
    }
    public void giverockets(int rounds)
    {
        if (rocketAmmo < rocketAmmoMax)
        {
            rocketAmmo += rounds;
        }
        if (rocketAmmo > rocketAmmoMax)
        {
            rocketAmmo = rocketAmmoMax;
        }
        updatePlayerRounds();

    }
    public void updatePlayerRounds()
    {
        //rocketAmmo = rocketAmmoMax;
        float rocketPercent = ((float)rocketAmmo / (float)rocketAmmoMax);
        gameManager.instance.shotgunAmmo.text = rocketPercent.ToString("F0");
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

        aud.PlayOneShot(playerHurt[Random.Range(0, playerHurt.Length)], playerHurtVol);//new

        updatePlayerHP();
        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            //kill player
            playerDeathCount++;
            gameManager.instance.totalDeaths.text = playerDeathCount.ToString("F0");
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
        float hpPercent = ((float)HP / (float)HPOrig * 100);
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;
        gameManager.instance.HPpercent.text = hpPercent.ToString("F0");
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

