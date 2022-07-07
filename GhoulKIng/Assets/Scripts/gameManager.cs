using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;
    public GameObject player;
    public playerController playerScript;

    public GameObject pausemenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    //public Image HPBar;

    public bool paused = false;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf)
        {

            if (!paused)
            {
                paused = true;
                pausemenu.SetActive(true);
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
        pausemenu.SetActive(false);
        playerDeadMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void playerDead()
    {
        paused = true;
        playerDeadMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

}
