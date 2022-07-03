using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour, IDamagable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;

    [Header("------------------------------")]
    [Header("Player Attributes")]
    [Range(5, 20)][SerializeField] int HP;
    [Range(3, 6)] [SerializeField] float playerSpeed;
    [Range(1.5f, 4)] [SerializeField] int SprintMulti;
    [Range(6, 10)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
    [Range(1, 4)] [SerializeField] int jumps;


    [Header("------------------------------")]
    [Header("Player Weapon Stats")]
    [SerializeField] float shootRate;

    [SerializeField] GameObject cube;

    [SerializeField] int weaponDamage;
    [Header("------------------------------")]
    [Header("Effects")]
    [SerializeField] GameObject hitEffectSpark;

    bool isSprinting = false;
    float playerSpeedOrig;
    int timesJumped;
    Vector3 playerVelocity;
    Vector3 move;
    bool canShoot = true;

        private void Start()
        {
        playerSpeedOrig = playerSpeed;
        }


        void Update()
        {
         movePlayer();
         Sprint();
         StartCoroutine(shoot());
        }
    private void movePlayer() 
    {
      if (controller.isGrounded && playerVelocity.y < 0)
            {
                timesJumped = 0;
                playerVelocity.y = 0f;
            }

            move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            // makes the player jump and increments the time they jump
            if (Input.GetButtonDown("Jump") && timesJumped < jumps)
            {
                timesJumped++;
                playerVelocity.y = jumpHeight;
            }
            //add gravity
            playerVelocity.y -= gravityValue * Time.deltaTime;

          //add gravity back into the character controller move
            controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint() 
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * SprintMulti;

        }
        else if(Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }

    IEnumerator shoot()
    {
       

        if (Input.GetButton("Shoot") && canShoot)
        {
         canShoot = false;

         RaycastHit hit;

         
            if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
            Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
               
                 if(hit.collider.GetComponent<enemyAI>() != null)
                 {
                    IDamagable isDamagable = hit.collider.GetComponent <IDamagable>();

                    if (hit.collider is SphereCollider)
                    {
                        isDamagable.takeDamage(10000);
                    }
                    else
                    {
                        isDamagable.takeDamage(weaponDamage);
                    }
                       
                 }


            }

         yield return new WaitForSeconds(shootRate);
         canShoot = true;
        }
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
           //Kill player
        }
    }
}

