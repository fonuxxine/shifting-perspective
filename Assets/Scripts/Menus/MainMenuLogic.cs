using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void Update()
    {
        if (Input.anyKey)
        {
            StartGame();
        }
    }

    public void StartGame() {
        SceneLoader.LoadScene(SceneID.Intro1); // load the first level
    }
}
