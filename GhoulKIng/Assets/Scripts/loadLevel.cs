using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadLevel : MonoBehaviour
{
    public bool showCase;
    public bool level1;
    public bool level2;
    public bool level3;
    public bool gameComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showCase == true)
                gameManager.instance.loadShowcase();
            else if (level1 == true)
                gameManager.instance.loadLevelOne();
            else if (level2 == true)
                gameManager.instance.loadLevelTwo();
            else if (level3 == true)
                gameManager.instance.loadLevelThree();
            else if (gameComplete == true)
                gameManager.instance.checkKeys();
        }
    }
}
