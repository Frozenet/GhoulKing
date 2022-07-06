using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.resume();
    }
    public void quit()
    {
        Application.Quit();
    }
    public void givePlayerHP(int amount)
    {
        gamemanager.instance.playerScript.giveHP(amount);
    }
    public void respawn()
    {
        gamemanager.instance.playerScript.respawn();
        gamemanager.instance.resume();

    }
}
