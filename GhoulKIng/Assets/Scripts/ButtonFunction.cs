using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.resume();
    }
    public void quit()
    {
        Application.Quit();
    }
    public void givePlayerHP(int amount)
    {
        gameManager.instance.playerScript.giveHP(amount);
    }
    public void respawn()
    {
        gameManager.instance.playerScript.respawn();
        gameManager.instance.restart();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.restart();
    }
}
