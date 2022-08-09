using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] public int num;

    public void givePlayerkey(int unlocked)
    {
        //gameManager.instance.keysGoal(unlocked);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Was Player Specific");
            gameManager.instance.updateKeyNumber();//this works no touch
            Destroy(gameObject);
        }
        
    }
}
