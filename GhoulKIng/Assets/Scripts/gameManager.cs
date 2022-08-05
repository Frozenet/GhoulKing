using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    [HideInInspector] public static gameManager instance;

    [Header("Player Reference")]
    public GameObject player;
    public playerController playerScript;
    public weaponSwap playerWeaponSwap;

    [Header("-----------------")]
    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    public GameObject winGameMenu;
    public GameObject titleScreen;
    public GameObject settingsMenu;
    public Image HPBar;
    public TMP_Text enemyDead;
    public TMP_Text enemyTotal;
    public TMP_Text keysHeld;
    public TMP_Text KeysTotal;
    public TMP_Text HPpercent;
    public TMP_Text shotgunAmmo;
    public TMP_Text shotgunAmmoMax;
    public TMP_Text rocketAmmo;
    public TMP_Text rocketAmmoMax;

    [HideInInspector] public bool paused = false;
    public GameObject menuCurrentlyOpen;
    public GameObject prevOpenMenu;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool titleScreenOn;

    int enemiesKilled;
    public int enemyKillGoal;
    [SerializeField] int KeysGoal;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerWeaponSwap = player.GetComponentInChildren<weaponSwap>();

        ////pause game when started up          requires debuggins
        //paused = true;
        //menuCurrentlyOpen = titleScreen;
        //titleScreenOn = true;
        //menuCurrentlyOpen.SetActive(true);
        //lockCursorPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameOver && !titleScreenOn)
        {
            if (!paused && !menuCurrentlyOpen)
            {
                paused = true;
                menuCurrentlyOpen = pauseMenu;
                menuCurrentlyOpen.SetActive(true);
                lockCursorPause();
            }
            else
            {
                resume();
            }

        }
    }

    public void updateAmmo()
    {

        
        shotgunAmmoMax.text = playerScript.shotgunAmmoMax.ToString("F0");
        rocketAmmoMax.text = playerScript.rocketAmmoMax.ToString("F0");

        shotgunAmmo.text = playerScript.shotgunAmmo.ToString("F0");
        rocketAmmo.text = playerScript.rocketAmmo.ToString("F0");
        
    }


    public void resume()
    {
        paused = false;
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = null;
        unlockCursorUnpause();
    }

    public void playerDead()
    {
        paused = true;
        menuCurrentlyOpen = playerDeadMenu;
        menuCurrentlyOpen.SetActive(true);
        lockCursorPause();
    }

    public void checkEnemyKills()
    {
        enemiesKilled++;
        enemyDead.text = enemiesKilled.ToString("F0");

        if (enemiesKilled >= enemyKillGoal)
        {
            menuCurrentlyOpen = winGameMenu;
            menuCurrentlyOpen.SetActive(true);
            gameOver = true;
            lockCursorPause();
        }
    }
    public void updateEnemyNumber()
    {
        enemyKillGoal++;
        enemyTotal.text = enemyKillGoal.ToString("F0");
    }
    
    //public void checkKeys()
    //{
    //    keysHeld.text = playerScript.keys.ToString("F0");
    //    if (playerScript.keys >= KeysGoal)
    //    {
    //        menuCurrentlyOpen = winGameMenu;
    //        menuCurrentlyOpen.SetActive(true);
    //        gameOver = true;
    //        lockCursorPause();
    //    }
    //}

    //public void updateKeysNumber()
    //{
    //    keysHeld.text = playerScript.keys.ToString("F0");
    //}
    public void restart()
    {
        gameOver = false;
        paused = false;
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = null;
        unlockCursorUnpause();
    }

    public void lockCursorPause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void unlockCursorUnpause()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void settings()
    {
        prevOpenMenu = menuCurrentlyOpen;
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = settingsMenu;
        menuCurrentlyOpen.SetActive(true);
    }
    public void back()
    {
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = prevOpenMenu;
        menuCurrentlyOpen.SetActive(true);
    }
}
