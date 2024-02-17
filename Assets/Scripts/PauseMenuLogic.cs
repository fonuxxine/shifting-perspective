using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    private bool paused;
    public GameObject menu;
    
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Running");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                unpauseGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void unpauseGame()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }
}
