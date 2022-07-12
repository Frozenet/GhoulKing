using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;
    public GameObject player;
    public playerController playerScript;

    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    public GameObject winGameMenu;

    public Image HPBar;

    public bool paused = false;

    int enemiesKilled;
    public int enemyKillGoal;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winGameMenu.activeSelf)
        {

            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {

                resume();
            }

        }
    }
    public void resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        winGameMenu.SetActive(false);
        playerDeadMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void playerDead()
    {
        paused = true;
        playerDeadMenu.SetActive(true);
        //playerDamageFlash.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void checkEnemyKills()
    {
        enemiesKilled++;
        if(enemiesKilled >= enemyKillGoal)
        {
            //show win game popup
            winGameMenu.SetActive(true);
            paused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
