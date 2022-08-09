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
    public GameObject loadLevelMenu;

    [Header("-----------------")]
    [Header("Health")]
    public Image HPBar;
    public TMP_Text HPpercent;

    [Header("-----------------")]
    [Header("Stats")]
    //public TMP_Text enemyDead;
    //public TMP_Text enemyTotal;
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

    [Header("-----------------")]
    [Header("Audio")]
    public AudioSource audi;
    [SerializeField] AudioClip[] menuClick;
    [Range(0, 1)] [SerializeField] float clickVol;

    [Header("-----------------")]
    [HideInInspector] public bool paused = false;
    public GameObject menuCurrentlyOpen;
    public GameObject prevOpenMenu;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool titleScreenOn;

    [Header("-----------------")]
    int enemiesKilled = 0;
    public int enemyKillGoal;
    public int keysCollected = 0;
    public int keysGoal;
    //[SerializeField] int KeysGoals;

    public GameObject titleScreenCam;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //sets the title screen to active
        if (SceneManager.GetActiveScene().name == "Show case level")//based on first level of game
        {
            menuCurrentlyOpen = titleScreen;
            titleScreenOn = true;
            gameOver = true;
        }


        //finds player components
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerWeaponSwap = player.GetComponentInChildren<weaponSwap>();

        //makes title screen operational
        if (SceneManager.GetActiveScene().name == "Show case level")//based on first level of game
        {
            titleScreenCam.SetActive(true);
            player.SetActive(false);
            lockCursorPause();
        }
        else
            unlockCursorUnpause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameOver)
        {
            if (!paused && !menuCurrentlyOpen)
            {
                audi.PlayOneShot(menuClick[0], clickVol);
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
        audi.PlayOneShot(menuClick[0], clickVol);
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
        //enemyDead.text = enemiesKilled.ToString("F0");
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
        //enemyTotal.text = enemyKillGoal.ToString("F0");
    }
    public void checkKeys()
    {
        keysHeld.text = keysCollected.ToString("F0");
        totalKeys.text = keysCollected.ToString("F0");
        loadMenuCondition();
    }
    public void updateKeyNumber()
    {
        keysCollected++;
        keysHeld.text = keysCollected.ToString("F0");
        totalKeys.text = keysCollected.ToString("F0");
    }

    public void restart()
    {
        audi.PlayOneShot(menuClick[0], clickVol);
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
        audi.PlayOneShot(menuClick[0], clickVol);
        prevOpenMenu = menuCurrentlyOpen;
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = settingsMenu;
        menuCurrentlyOpen.SetActive(true);
    }
    public void back()
    {
        //goes back to the previous open menu
        audi.PlayOneShot(menuClick[0], clickVol);
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = prevOpenMenu;
        menuCurrentlyOpen.SetActive(true);
    }
    public void creditsContinue()
    {
        //changes to credits scene
        audi.PlayOneShot(menuClick[0], clickVol);
        menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = creditsScreen;
        menuCurrentlyOpen.SetActive(true);
    }
    public void titleScreenBTN()//debugging required
    {
        //change scene name to final game lvl
        //reloads the scene
        audi.PlayOneShot(menuClick[0], clickVol);
        loadShowcase();
    }
    public void startBTN()
    {
        //changes from title screen to game screen
        audi.PlayOneShot(menuClick[0], clickVol);
        gameOver = false;
        titleScreenOn = false;
        titleScreen.SetActive(false);
        titleScreenCam.SetActive(false);
        player.SetActive(true);
        unlockCursorUnpause();
    }
    public void loadShowcase()
    {
        loadMenuCondition();
        SceneManager.LoadScene("Show case level");
    }
    public void loadLevelOne()
    {
        if (keysCollected >= keysGoal)
        {
            loadMenuCondition();
            SceneManager.LoadScene("Terrain level");
        }
    }
    public void loadLevelTwo()
    {
        if (keysCollected >= keysGoal)
        {
            loadMenuCondition();
            SceneManager.LoadScene("CorridorLevelOne");
        }
    }
    public void loadLevelThree()
    {
        if (keysCollected >= keysGoal)
        {
            loadMenuCondition();
            SceneManager.LoadScene("CorridorLevelTwo");
        }
    }
    public void loadLevelFour()
    {
        if (keysCollected >= keysGoal)
        {
            loadMenuCondition();
            SceneManager.LoadScene("CorridorLevelThree");
        }
    }
    public void loadLevelFive()
    {
        if (keysCollected >= keysGoal)
        {
            loadMenuCondition();
            SceneManager.LoadScene("FinalLevel");
        }
    }
    public void winMenuCondition()
    {
        menuCurrentlyOpen = winGameMenu;
        menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        lockCursorPause();
    }
    public void loadMenuCondition()
    {
        menuCurrentlyOpen = loadLevelMenu;
        menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        lockCursorPause();
    }
}
