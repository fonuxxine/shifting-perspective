using UnityEngine;

public class CreditsSkip : MonoBehaviour
{
    public void Update()
    {
        if (Input.anyKey)
        {
            StartGame();
        }
    }

    public void StartGame() {
        SceneLoader.LoadScene(SceneID.MainMenu); // load the main menu
    }
}
