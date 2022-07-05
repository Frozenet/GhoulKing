using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamagable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;

    [Header("Player Attributes")]
    [Header("------------------------------")]
    [Range(5, 20)] [SerializeField] int HP;
    [Range(3, 6)] [SerializeField] float playerSpeed;
    [Range(1.5f, 4)] [SerializeField] float sprintMulti;
    [Range(6, 10)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
    [Range(1, 4)] [SerializeField] int jumps;


    [Header("Player Weapon Stats")]
    [Header("------------------------------")]
    [Range(0.1f, 3)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int weaponDamage;

    [Header("Effects")]
    [Header("------------------------------")]
    [SerializeField] GameObject hitEffectSpark;
    [SerializeField] GameObject muzzleFlash;

    bool isSprinting = false;
    float playerSpeedOrig;
    int timesJumped;
    Vector3 playerVelocity;
    Vector3 move;
    bool canShoot = true;
    int HPOrig;
    Vector3 playerSpawnPos;

    private void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        playerSpawnPos = transform.position;
    }

    void Update()
    {
        if (!gameManager.instance.paused)
        {
            movePlayer();
            sprint();
            StartCoroutine(shoot());
        }
    }

    private void movePlayer()
    {
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            playerVelocity.y -= 3;
        }

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            timesJumped = 0;
            playerVelocity.y = 0f;
        }

        move = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // make the player jump
        if (Input.GetButtonDown("Jump") && timesJumped < jumps)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }

        //add gravity back into the character controller move
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMulti;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }

    IEnumerator shoot()
    {
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red); // for a line to be drawn for hitscan

        if (Input.GetButton("Shoot") && canShoot)
        {
            canShoot = false;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0)), out hit))
            {
                Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
                IDamagable isDamageable = hit.collider.GetComponent<IDamagable>();

                if (hit.collider.GetComponent<IDamagable>() != null)
                {
                    if (hit.collider is SphereCollider)
                        isDamageable.takeDamage(weaponDamage * 2);
                    else
                        isDamageable.takeDamage(weaponDamage);
                }
            }
            muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(.05f);
            muzzleFlash.SetActive(false);

            yield return new WaitForSeconds(shootRate);
            canShoot = true;
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        updatePlayerHP();

        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            gameManager.instance.playerDead();
        }
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);
    }

    public void giveHP(int amount)
    {
        if (HP < HPOrig)
            HP += amount;

        if (HP > HPOrig)
            HP = HPOrig;

        updatePlayerHP();
    }

    public void updatePlayerHP()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;
    }

    public void respawn()
    {
        HP = HPOrig;
        updatePlayerHP();
        controller.enabled = false;
        transform.position = playerSpawnPos;
        controller.enabled = true;
    }
}
