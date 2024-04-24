using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void Update()
    {
        if (!Input.anyKey) return;
        if (!Input.GetButtonDown("Cancel"))
        {
            StartGame();
        }
        else
        {
            Application.Quit();
        }
    }

    public void StartGame() {
        SceneLoader.LoadScene(SceneID.Intro1); // load the first level
    }
}
