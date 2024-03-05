using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100;
    // Start is called before the first frame update
    public void AddHealth(float amount)
    {
        playerHealth += amount;
    }

    // Update is called once per frame
    public void DecreaseHealth(float amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
    }
}
