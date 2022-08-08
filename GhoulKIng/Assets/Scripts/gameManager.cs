using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [HideInInspector] public static gameManager instance;

    [Header("Player Reference")]
    public GameObject player;
    public playerController playerScript;
    public weaponSwap playerWeaponSwap;

    [Header("-----------------")]
    [Header("UI")]
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    public GameObject winGameMenu;
    public GameObject titleScreen;
    public GameObject settingsMenu;
    public GameObject creditsScreen;

    [Header("-----------------")]
    [Header("Health")]
    public Image HPBar;
    public TMP_Text HPpercent;

    [Header("-----------------")]
    [Header("Stats")]
    public TMP_Text enemyDead;
    public TMP_Text enemyTotal;
    public TMP_Text keysHeld;
    public TMP_Text KeysTotal;

    public TMP_Text totalDeaths;
    public TMP_Text totalKilled;
    public TMP_Text totalKeys;

    [Header("-----------------")]
    [Header("Weapons")]
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
    int keysCollected;
    public int keysGoal;
    //[SerializeField] int KeysGoals;

    public GameObject titleScreenCam;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //sets the title screen to active
        menuCurrentlyOpen = titleScreen;
        titleScreenOn = true;
        gameOver = true;

        //finds player components
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerWeaponSwap = player.GetComponentInChildren<weaponSwap>();

        //makes title screen operational
        titleScreenCam.SetActive(true);
        player.SetActive(false);
        lockCursorPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameOver)
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
        gameOver = true;
        paused = true;
        menuCurrentlyOpen = playerDeadMenu;
        menuCurrentlyOpen.SetActive(true);
        lockCursorPause();
    }

    public void checkEnemyKills()
    {
        enemiesKilled++;
        enemyDead.text = enemiesKilled.ToString("F0");
        totalKilled.text = enemiesKilled.ToString("F0");

        //if (enemiesKilled >= enemyKillGoal)
        //{
        //    menuCurrentlyOpen = winGameMenu;
        //    menuCurrentlyOpen.SetActive(true);
        //    gameOver = true;
        //    lockCursorPause();
        //}
    }
    public void updateEnemyNumber()
    {
        enemyKillGoal++;
        enemyTotal.text = enemyKillGoal.ToString("F0");
    }
    public void checkKeys()
    {
        keysCollected++;
        keysHeld.text = keysCollected.ToString("F0");
        KeysTotal.text = keysCollected.ToString("F0");
        if (keysCollected >= keysGoal)
        {
            menuCurrentlyOpen = winGameMenu;
            menuCurrentlyOpen.SetActive(true);
            gameOver = true;
            lockCursorPause();
        }
    }
    public void updateKeyNumber()
    {
        keysGoal++;
        //keysHeld.text = playerScript.keys.ToString("F0");
        KeysTotal.text = keysGoal.ToString("F0");
    }

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
        //used when in the menus
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void unlockCursorUnpause()
    {
        //used then controlling the player
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void settings()
    {
        //opens the settings menu
        prevOpenMenu = menuCurrentlyOpen;
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = settingsMenu;
        menuCurrentlyOpen.SetActive(true);
    }
    public void back()
    {
        //goes back to the previous open menu
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = prevOpenMenu;
        menuCurrentlyOpen.SetActive(true);
    }
    public void creditsContinue()
    {
        //changes to credits scene
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = creditsScreen;
        menuCurrentlyOpen.SetActive(true);
    }
    public void titleScreenBTN()//debugging required
    {
        //change scene name to final game lvl
        //reloads the scene
        loadShowcase();
    }
    public void startBTN()
    {
        //changes from title screen to game screen
        gameOver = false;
        titleScreenOn = false;
        titleScreen.SetActive(false);
        titleScreenCam.SetActive(false);
        player.SetActive(true);
        unlockCursorUnpause();
    }
    public void loadShowcase()
    {
        SceneManager.LoadScene("Show case level");
    }
    public void loadLevelOne()
    {
        SceneManager.LoadScene("Terrain level");
    }
    public void loadLevelTwo()
    {
        SceneManager.LoadScene("");
    }
    public void loadLevelThree()
    {
        SceneManager.LoadScene("");
    }
}
