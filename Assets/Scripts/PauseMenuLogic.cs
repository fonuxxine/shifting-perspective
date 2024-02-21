using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    private bool paused;
    public GameObject menu;
    
    // start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        paused = false;
    }

    // update is called once per frame
    void Update()
    {
        Debug.Log("Running");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                pauseGame();
            }
            else
            {
                unpauseGame();
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

    public void restartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void backToMainMenu()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(2);
    }
}
