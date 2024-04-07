using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 3;

    public Image[] hearts;

    public Sprite fullHeart;

    public Sprite emptyHeart;
    
    // Reset player's health
    public void ResetHealth()
    {
        playerHealth = 3;
    }

    // Decrease player's health
    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
    }

    private void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        for (int i = 0; i < playerHealth; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
