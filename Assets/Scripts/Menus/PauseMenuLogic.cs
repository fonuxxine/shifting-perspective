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
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneLoader.LoadSceneByBuildIndex(currentSceneIndex);
    }

    public void backToMainMenu()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneLoader.LoadScene(SceneID.MainMenu);
    }

    // --- The below function was added for the Alpha demo ---
    public void skipLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.name == SceneLoader.GetSceneName(SceneID.ExtLevel))
        {
            Time.timeScale = 1;
            SceneLoader.LoadScene(SceneID.IntTutLevel);
        } else if (currentScene.name == SceneLoader.GetSceneName(SceneID.IntTutLevel))
        {
            Time.timeScale = 1;
            SceneLoader.LoadScene(SceneID.IntLevel1);
        } else if (currentScene.name == SceneLoader.GetSceneName(SceneID.IntLevel1))
        {
            Time.timeScale = 1;
            SceneLoader.LoadScene(SceneID.ToBeContinued);
        }
    }
}
